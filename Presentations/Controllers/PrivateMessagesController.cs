using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.PrivateMessageImages;
using DataAccesses.DTOs.PrivateMessages;
using DataAccesses.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
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
        public async Task<IActionResult> GetPrivateMsgesPagination(int page, int items, int sender, int receiver)
        {
            var result = _unitOfWork.PrivateMsges.GetPrivateMsgesPagination(page, items, sender, receiver);
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
            var privateMessage = _mapper.Map<PrivateMessage>(model);
            /* Insert private message */
            _unitOfWork.PrivateMsges.Insert(privateMessage);
            _unitOfWork.Save();
            /* Insert images */
            var images = new List<string>();
            if (model.Images != null && model.Images.Any())
            {
                var privateMessageImagesDto = new List<CreatePrivateMessageImageDTO>();
                foreach (var image in model.Images)
                {
                    var imageUrl = await _cloudinaryService.UploadImage(image);
                    privateMessageImagesDto.Add(new CreatePrivateMessageImageDTO
                    {
                        MessageId = privateMessage.MessageId,
                        ImageUrl = imageUrl
                    });
                }
                if (privateMessageImagesDto.Any())
                {
                    var privateMessageImages = _mapper.Map<List<PrivateMessageImage>>(privateMessageImagesDto);
                    _unitOfWork.PrivateMessageImages.InsertRange(privateMessageImages);
                    _unitOfWork.Save();
                }
            }
            _unitOfWork.Commit();
            return Ok(new GetPrivateMessageDTO
            {
                MessageId = privateMessage.MessageId,
                UserId = privateMessage.UserId,
                Avatar = _unitOfWork.Users.GetById(privateMessage.UserId).Avatar,
                UserName = _unitOfWork.Users.GetById(privateMessage.UserId).UserName,
                Receiver = privateMessage.Receiver,
                Content = privateMessage.Content,
                Images = privateMessage.Images?.Select(i => new GetPrivateMessageImageDTO
                {
                    ImageId = i.ImageId,
                    ImageUrl = i.ImageUrl,
                    MessageId = i.MessageId,
                })?.ToList() ?? new List<GetPrivateMessageImageDTO>(),
                DateCreated = privateMessage.DateCreated.ToString("yyyy-MM-dd HH:mm"),
            });
        }

        // PUT api/<PrivateMessagesController>/5
        [HttpPut]
        public async Task<IActionResult> UpdatePrivateMessage([FromForm] UpdatePrivateMessageDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.BeginTransaction();
            var privateMessage = _unitOfWork.PrivateMsges.GetById(model.PrivateMessageId);
            if (privateMessage == null)
            {
                return NotFound();
            }
            /* update message */
            _mapper.Map(model, privateMessage);
            _unitOfWork.PrivateMsges.Update(privateMessage);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(new GetPrivateMessageDTO
            {
                MessageId = privateMessage.MessageId,
                UserId = privateMessage.UserId,
                Avatar = _unitOfWork.Users.GetById(privateMessage.UserId).Avatar,
                UserName = _unitOfWork.Users.GetById(privateMessage.UserId).UserName,
                Receiver = privateMessage.Receiver,
                Content = privateMessage.Content,
                Images = privateMessage.Images?.Select(i => new GetPrivateMessageImageDTO
                {
                    ImageId = i.ImageId,
                    ImageUrl = i.ImageUrl,
                    MessageId = i.MessageId,
                })?.ToList() ?? new List<GetPrivateMessageImageDTO>(),
                DateCreated = privateMessage.DateCreated.ToString("yyyy-MM-dd HH:mm"),
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
            var privateMessage = _unitOfWork.PrivateMsges.GetById(messageId);
            if (privateMessage == null)
            {
                return NotFound();
            }
            /* delete images */
            if (privateMessage.Images != null && privateMessage.Images.Any())
            {
                foreach (var image in privateMessage.Images)
                {
                    var publicId = _cloudinaryService.GetImagePublicId(image.ImageUrl);
                    await _cloudinaryService.DeleteImage(publicId);
                    _unitOfWork.PrivateMessageImages.Delete(image.ImageId);
                }
            }
            _unitOfWork.PrivateMsges.Delete(messageId);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return NoContent();
        }
    }
}
