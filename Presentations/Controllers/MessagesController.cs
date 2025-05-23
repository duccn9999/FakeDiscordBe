using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.LastSeenMessages;
using DataAccesses.DTOs.MentionUsers;
using DataAccesses.DTOs.MessageAttachments;
using DataAccesses.DTOs.Messages;
using DataAccesses.Models;
using DataAccesses.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryRepository _cloudinaryService;
        public MessagesController(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryRepository cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }
        // GET: api/<MessagesController>
        [HttpGet("GetMessages/{channelId}")]
        [Authorize(Policy = "VIEW_MESSAGES")]
        public async Task<IActionResult> GetMessage(int channelId)
        {
            var result = await _unitOfWork.Messages.GetMessagesByChannelId(channelId);
            return Ok(result);
        }

        [HttpGet("GetMessagesPagination/{channelId}")]
        [Authorize(Policy = "VIEW_MESSAGES")]
        public async Task<IActionResult> GetMessagesPagination(int channelId, int page, int itemsPerPage)
        {
            var result = await _unitOfWork.Messages.GetMessagesPaginationByChannelId(channelId, page, itemsPerPage);
            return Ok(result);
        }
        [HttpGet("GetPrivateMessages/{channelId}")]
        public async Task<IActionResult> GetPrivateMessage(int channelId)
        {
            var result = await _unitOfWork.Messages.GetMessagesPaginationByPrivateChannelId(channelId);
            return Ok(result);
        }
        // POST api/<MessagesController>
        [Authorize(Policy = Permissions.CAN_SEND_MESSAGES)]
        [HttpPost("CreateMessage")]
        public async Task<IActionResult> Post([FromForm] CreateMessageDTO model)
        {
            _unitOfWork.BeginTransaction();
            var message = _mapper.Map<Message>(model);
            var insertProcess = _unitOfWork.Messages.InsertAsync(message);
            await insertProcess;
            _unitOfWork.Save();
            /* Insert attachments */
            if (model.Attachments != null && model.Attachments.Any())
            {
                var attachmentDtos = new List<CreateMessageAttachmentDTO>();
                foreach (var image in model.Attachments)
                {
                    var attachment = await _cloudinaryService.UploadAttachment(image);
                    attachmentDtos.Add(new CreateMessageAttachmentDTO
                    {
                        MessageId = message.MessageId,
                        Url = attachment.Url,
                        PublicId = attachment.PublicId,
                        ContentType = attachment.ResourceType,
                        DisplayName = attachment.DisplayName,
                        OriginalFilename = attachment.OriginalFilename,
                        DownloadLink = _cloudinaryService.GetDownloadLink(attachment.Url, attachment.OriginalFilename, attachment.DisplayName),
                    });
                }
                if (attachmentDtos.Any())
                {
                    var attachmentsModel = _mapper.Map<List<MessageAttachment>>(attachmentDtos);
                    _unitOfWork.MessageAttachments.InsertRange(attachmentsModel);
                    _unitOfWork.Save();
                }
            }
            /* Insert mentions*/
            if(model.MentionUsers != null && model.MentionUsers.Any())
            {
                var mentionUserDtos = new List<CreateMentionUserDTO>();
                foreach (var userId in model.MentionUsers)
                {
                    mentionUserDtos.Add(new CreateMentionUserDTO
                    {
                        MessageId = message.MessageId,
                        UserId = userId
                    });
                }
                if (mentionUserDtos.Any())
                {
                    var mentionUsers = _mapper.Map<List<MentionUser>>(mentionUserDtos);
                    _unitOfWork.MentionUsers.InsertRange(mentionUsers);
                    _unitOfWork.Save();
                }
            }
            _unitOfWork.Commit();
            var user = await _unitOfWork.Users.GetByIdAsync(message.UserCreated);
            var attachments = _unitOfWork.MessageAttachments.GetAll().Where(attachment => attachment.MessageId == message.MessageId);
            return Created("CreateMessage", new GetMessageDTO
            {
                MessageId = message.MessageId,
                Username = user.UserName,
                Avatar = user.Avatar,
                Content = message.Content,
                DateCreated = message.DateCreated.ToString("yyyy-MM-dd HH:mm"),
                DateModified = message.DateModified,
                ChannelId = message.ChannelId,
                Attachments = attachments.Select(i => new GetMessageAttachmentDTO
                {
                    MessageId = i.MessageId,
                    AttachmentId = i.AttachmentId,
                    Url = i.Url,
                    PublicId = i.PublicId,
                    DisplayName = i.DisplayName,
                    ContentType = i.ContentType,
                    DownloadLink = i.DownloadLink,
                }).ToList()
            });
        }

        // PUT api/<MessagesController>/5
        [HttpPut("UpdateMessage")]
        public async Task<IActionResult> Put([FromBody] UpdateMessageDTO model)
        {
            _unitOfWork.BeginTransaction();
            var message = await _unitOfWork.Messages.GetByIdAsync(model.MessageId);
            _mapper.Map(model, message);
            _unitOfWork.Messages.Update(message);
            /* update mentions */
            var mentionUsers = _unitOfWork.MentionUsers.GetAll().Where(x => x.MessageId == model.MessageId);
            if (mentionUsers != null && mentionUsers.Any())
            {
                _unitOfWork.MentionUsers.DeleteRange(mentionUsers.ToList());
            }
            if (model.MentionUsers != null && model.MentionUsers.Any())
            {
                var mentionUserDtos = new List<CreateMentionUserDTO>();
                foreach (var userId in model.MentionUsers)
                {
                    mentionUserDtos.Add(new CreateMentionUserDTO
                    {
                        MessageId = message.MessageId,
                        UserId = userId
                    });
                }
                if (mentionUserDtos.Any())
                {
                    var mentionUsersModel = _mapper.Map<List<MentionUser>>(mentionUserDtos);
                    _unitOfWork.MentionUsers.InsertRange(mentionUsersModel);
                    _unitOfWork.Save();
                }
            }
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var user = await _unitOfWork.Users.GetByIdAsync(message.UserCreated);
            var attachments = _unitOfWork.MessageAttachments.GetAll().Where(attachment => attachment.MessageId == message.MessageId);
            return Ok(new GetMessageDTO
            {
                MessageId = message.MessageId,
                Username = user.UserName,
                Avatar = user.Avatar,
                Content = message.Content,
                DateCreated = message.DateCreated.ToString("yyyy-MM-dd HH:mm"),
                DateModified = message.DateModified,
                ChannelId = message.ChannelId,
                Attachments = attachments.Select(i => new GetMessageAttachmentDTO
                {
                    MessageId = i.MessageId,
                    AttachmentId = i.AttachmentId,
                    Url = i.Url,
                    PublicId = i.PublicId,
                    DisplayName = i.DisplayName,
                    ContentType = i.ContentType,
                    DownloadLink = i.DownloadLink,
                }).ToList()
            });
        }

        // DELETE api/<MessagesController>/5
        [HttpDelete("DeleteMessage/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _unitOfWork.BeginTransaction();
            var message = _unitOfWork.Messages.GetById(id);
            var attachments = _unitOfWork.MessageAttachments.GetAll().Where(image => image.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }
            /* delete attachments */
            if (attachments != null && attachments.Any())
            {
                foreach (var attachment in attachments)
                {
                    await _cloudinaryService.DeleteAttachment(attachment.PublicId);
                    _unitOfWork.MessageAttachments.Delete(attachment.AttachmentId);
                }
            }
            _unitOfWork.Save();
            _unitOfWork.Messages.Delete(id);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var user = await _unitOfWork.Users.GetByIdAsync(message.UserCreated);
            return Ok(new GetMessageDTO
            {
                MessageId = message.MessageId,
                Username = user.UserName,
                Avatar = user.Avatar,
                Content = message.Content,
                DateCreated = message.DateCreated.ToString("yyyy-MM-dd HH:mm"),
                DateModified = message.DateModified,
                ChannelId = message.ChannelId,
                Attachments = attachments.Select(i => new GetMessageAttachmentDTO
                {
                    MessageId = i.MessageId,
                    AttachmentId = i.AttachmentId,
                    Url = i.Url,
                    PublicId = i.PublicId,
                    DisplayName = i.DisplayName,
                    ContentType = i.ContentType,
                    DownloadLink = i.DownloadLink,
                }).ToList()
            });
        }


        [HttpDelete("DeleteMessageAttachment/{attachmentId}")]
        public async Task<IActionResult> DeleteAttachment(int attachmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.BeginTransaction();
            var attachment = _unitOfWork.MessageAttachments.GetById(attachmentId);
            if (attachment == null)
            {
                return NotFound();
            }
            /* delete image */
            if (attachment != null)
            {
                await _cloudinaryService.DeleteAttachment(attachment.PublicId);
                _unitOfWork.MessageAttachments.Delete(attachment.AttachmentId);
            }
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var message = _unitOfWork.Messages.GetById(attachment.MessageId);
            var attachments = _unitOfWork.MessageAttachments.GetAll().Where(image => image.MessageId == message.MessageId);
            var user = _unitOfWork.Users.GetById(message.UserCreated);
            return Ok(new GetMessageDTO
            {
                MessageId = message.MessageId,
                Username = user.UserName,
                Avatar = user.Avatar,
                Content = message.Content,
                DateCreated = message.DateCreated.ToString("yyyy-MM-dd HH:mm"),
                DateModified = message.DateModified,
                ChannelId = message.ChannelId,
                Attachments = attachments.Select(i => new GetMessageAttachmentDTO
                {
                    MessageId = i.MessageId,
                    AttachmentId = i.AttachmentId,
                    Url = i.Url,
                    PublicId = i.PublicId,
                    DisplayName = i.DisplayName,
                    ContentType = i.ContentType,
                    DownloadLink = i.DownloadLink,
                }).ToList()
            });
        }

        [HttpPost("AddLastSeenMessage")]
        public async Task<IActionResult> AddLastSeenMessage(CreateLastSeenMessageDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.BeginTransaction();
            var lastSeenMessage = _unitOfWork.LastSeenMessages.GetAll().FirstOrDefault(x => x.UserId == model.UserId && x.ChannelId == model.ChannelId && x.MessageId == model.MessageId);
            if(lastSeenMessage != null)
            {
                return Ok(new { alreadySeen = true });
            }
            else
            {
                var newLastSeenMessage = _mapper.Map<LastSeenMessage>(model);
                _unitOfWork.LastSeenMessages.Insert(newLastSeenMessage);
                _unitOfWork.Save();
                _unitOfWork.Commit();
            }
            return Ok();
        }
        [HttpGet("GetLastSeenMessage/{userId}/{channelId}")]
        public async Task<IActionResult> GetLastSeenMessage(int userId, int channelId)
        {
            var lastSeenMessage = _unitOfWork.LastSeenMessages.GetAll().FirstOrDefault(x => x.UserId == userId && x.ChannelId == channelId);
            if (lastSeenMessage == null)
            {
                var newestMessageId = _unitOfWork.Messages.GetAll()
                    .Where(x => x.ChannelId == channelId)
                    .OrderByDescending(x => x.DateCreated)
                    .Select(x => x.MessageId)
                    .FirstOrDefault();
                return Ok(new GetLastSeenMessageDTO
                {
                    UserId = userId,
                    ChannelId = channelId,
                    MessageId = newestMessageId,
                    DateSeen = DateTime.Now
                });
            }
            return Ok(new GetLastSeenMessageDTO
            {
                UserId = lastSeenMessage.UserId,
                ChannelId = lastSeenMessage.ChannelId,
                MessageId = lastSeenMessage.MessageId,
                DateSeen = lastSeenMessage.DateSeen
            });
        }

        [HttpPut("UpdateLastSeenMessage/{userId}/{channelId}")]
        public async Task<IActionResult> UpdateLastSeenMessage(int userId, int channelId, [FromBody] int messageId)
        {
            var lastSeenMessage = _unitOfWork.LastSeenMessages.GetAll().FirstOrDefault(x => x.UserId == userId && x.ChannelId == channelId);
            if (lastSeenMessage == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            var lastSeenMessageDto = new CreateLastSeenMessageDTO
            {
                UserId = userId,
                ChannelId = channelId,
                MessageId = messageId,
            };
            var updatedLastSeenMessage = _mapper.Map(lastSeenMessageDto, lastSeenMessage);
            _unitOfWork.LastSeenMessages.Update(updatedLastSeenMessage);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(new GetLastSeenMessageDTO
            {
                UserId = updatedLastSeenMessage.UserId,
                ChannelId = updatedLastSeenMessage.ChannelId,
                MessageId = updatedLastSeenMessage.MessageId,
                DateSeen = updatedLastSeenMessage.DateSeen
            });
        }

        [HttpGet("GetMentionCountByUser/{userId}/{channelId}")]
        public async Task<IActionResult> GetMentionCountByUser(int userId, int channelId)
        {
            var result = await _unitOfWork.MentionUsers.GetMentionCountByUser(userId, channelId);
            return Ok(result);
        }
        [HttpPut("MarkMentionsAsRead/{userId}/{channelId}")]
        public async Task<IActionResult> MarkMentionsAsRead(int userId, int channelId)
        {
            _unitOfWork.BeginTransaction();
            await _unitOfWork.MentionUsers.MarkMentionsAsRead(userId);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var getMentionsCount = await _unitOfWork.MentionUsers.GetMentionCountByUser(userId, channelId);
            return Ok(getMentionsCount);
        }
    }
}
