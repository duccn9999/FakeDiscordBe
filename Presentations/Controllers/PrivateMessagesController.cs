using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.PrivateMessageAttachments;
using DataAccesses.DTOs.PrivateMessages;
using DataAccesses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PrivateMessagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        public PrivateMessagesController(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }
        // GET: api/<PrivateMessagesController>
        [HttpGet("{sender}/{receiver}")]
        public async Task<IActionResult> GetPrivateMsgesPagination(int sender, int receiver)
        {
            var result = _unitOfWork.PrivateMsges.GetPrivateMsges(sender, receiver);
            return Ok(result);
        }

        // GET api/<PrivateMessagesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PrivateMessagesController>
        [HttpPost]
        public async Task<IActionResult> CreatePrivateMessage([FromForm] CreatePrivateMessageDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.BeginTransaction();
            var message = _mapper.Map<PrivateMessage>(model);
            /* Insert private message */
            _unitOfWork.PrivateMsges.Insert(message);
            _unitOfWork.Save();
            /* Insert images */
            var images = new List<string>();
            if (model.Attachments != null && model.Attachments.Any())
            {
                var attachmentDtos = new List<CreatePrivateMessageAttachmentDTO>();
                foreach (var image in model.Attachments)
                {
                    var attachment = await _cloudinaryService.UploadAttachment(image);
                    attachmentDtos.Add(new CreatePrivateMessageAttachmentDTO
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
                    var attachmentsModel = _mapper.Map<List<PrivateMessageAttachment>>(attachmentDtos);
                    _unitOfWork.PrivateMessageAttachments.InsertRange(attachmentsModel);
                    _unitOfWork.Save();
                }
            }
            _unitOfWork.Commit();
            var attachments = _unitOfWork.PrivateMessageAttachments.GetAll().Where(attachment => attachment.MessageId == message.MessageId);
            return Ok(new GetPrivateMessageDTO
            {
                MessageId = message.MessageId,
                UserId = message.UserId,
                Avatar = _unitOfWork.Users.GetById(message.UserId).Avatar,
                UserName = _unitOfWork.Users.GetById(message.UserId).UserName,
                Receiver = message.Receiver,
                Content = message.Content,
                Attachments = attachments?.Select(i => new GetPrivateMessageAttachmentDTO
                {
                    AttachmentId = i.AttachmentId,
                    Url = i.Url,
                    MessageId = i.MessageId,
                    PublicId = i.PublicId,
                    DisplayName = i.DisplayName,
                    ContentType = i.ContentType,
                    DownloadLink = i.DownloadLink,
                })?.ToList() ?? new List<GetPrivateMessageAttachmentDTO>(),
                DateCreated = message.DateCreated.ToString("yyyy-MM-dd HH:mm"),
            });
        }

        // PUT api/<PrivateMessagesController>/5
        [HttpPut]
        public async Task<IActionResult> UpdatePrivateMessage([FromBody] UpdatePrivateMessageDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.BeginTransaction();
            var message = _unitOfWork.PrivateMsges.GetById(model.PrivateMessageId);
            if (message == null)
            {
                return NotFound();
            }
            /* update message */
            _mapper.Map(model, message);
            _unitOfWork.PrivateMsges.Update(message);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var user = await _unitOfWork.Users.GetByIdAsync(message.UserId);
            var attachments = _unitOfWork.PrivateMessageAttachments.GetAll().Where(image => image.MessageId == message.MessageId);
            return Ok(new GetPrivateMessageDTO
            {
                MessageId = message.MessageId,
                UserId = message.UserId,
                Avatar = user.Avatar,
                UserName = user.UserName,
                Receiver = message.Receiver,
                Content = message.Content,
                Attachments = attachments?.Select(i => new GetPrivateMessageAttachmentDTO
                {
                    AttachmentId = i.AttachmentId,
                    Url = i.Url,
                    MessageId = i.MessageId,
                    PublicId = i.PublicId,
                    ContentType = i.ContentType,
                    DisplayName = i.DisplayName,
                    DownloadLink = i.DownloadLink,
                })?.ToList() ?? new List<GetPrivateMessageAttachmentDTO>(),
                DateCreated = message.DateCreated.ToString("yyyy-MM-dd HH:mm"),
            });
        }

        // DELETE api/<PrivateMessagesController>/5
        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeletePrivateMessage(int messageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.BeginTransaction();
            var message = _unitOfWork.PrivateMsges.GetById(messageId);
            var attachments = _unitOfWork.PrivateMessageAttachments.GetAll().Where(image => image.MessageId == messageId);
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
                    _unitOfWork.PrivateMessageAttachments.Delete(attachment.AttachmentId);
                }
            }
            _unitOfWork.PrivateMsges.Delete(messageId);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var user = await _unitOfWork.Users.GetByIdAsync(message.UserId);
            return Ok(new GetPrivateMessageDTO
            {
                MessageId = message.MessageId,
                UserId = message.UserId,
                Avatar = user.Avatar,
                UserName = user.UserName,
                Receiver = message.Receiver,
                Content = message.Content,
                Attachments = attachments?.Select(i => new GetPrivateMessageAttachmentDTO
                {
                    AttachmentId = i.AttachmentId,
                    Url = i.Url,
                    MessageId = i.MessageId,
                    ContentType = i.ContentType,
                    PublicId = i.PublicId,
                    DisplayName = i.DisplayName,
                    DownloadLink = i.DownloadLink
                })?.ToList() ?? new List<GetPrivateMessageAttachmentDTO>(),
                DateCreated = message.DateCreated.ToString("yyyy-MM-dd HH:mm"),
            });
        }

        [HttpDelete("{attachmentId}")]
        public async Task<IActionResult> DeletePrivateMessageImage(int attachmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.BeginTransaction();
            var attachment = _unitOfWork.PrivateMessageAttachments.GetById(attachmentId);
            if (attachment == null)
            {
                return NotFound();
            }
            /* delete image */
            if (attachment != null)
            {
                await _cloudinaryService.DeleteAttachment(attachment.PublicId);
                _unitOfWork.PrivateMessageAttachments.Delete(attachment.AttachmentId);
            }
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var message = _unitOfWork.PrivateMsges.GetById(attachment.MessageId);
            var attachments = _unitOfWork.PrivateMessageAttachments.GetAll().Where(image => image.MessageId == message.MessageId);
            var user = _unitOfWork.Users.GetById(message.UserId);
            return Ok(new GetPrivateMessageDTO
            {
                MessageId = message.MessageId,
                UserId = message.UserId,
                Avatar = user.Avatar,
                UserName = user.UserName,
                Receiver = message.Receiver,
                Content = message.Content,
                Attachments = attachments?.Select(i => new GetPrivateMessageAttachmentDTO
                {
                    AttachmentId = i.AttachmentId,
                    Url = i.Url,
                    MessageId = i.MessageId,
                    ContentType = i.ContentType,
                    PublicId = i.PublicId,
                    DisplayName = i.DisplayName,
                    DownloadLink = i.DownloadLink,
                })?.ToList() ?? new List<GetPrivateMessageAttachmentDTO>(),
                DateCreated = message.DateCreated.ToString("yyyy-MM-dd HH:mm"),
            });
        }
    }
}
