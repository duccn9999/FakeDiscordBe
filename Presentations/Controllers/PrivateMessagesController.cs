using AutoMapper;
using BusinessLogics.Repositories;
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
        public PrivateMessagesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
        public async Task<IActionResult> CreatePrivateMessage([FromBody] CreatePrivateMessagesDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var privateMessagesDto = model.Contents.Select(content => new CreatePrivateMessageDTO
            {
                Receiver = model.Receiver,
                Content = content,
                DateCreated = DateTime.Now
            }).ToList();
            var privateMessages = _mapper.Map<List<PrivateMessage>>(privateMessagesDto);
            await _unitOfWork.PrivateMsges.InsertRangeAsync(privateMessages);
            _unitOfWork.Commit();
            return Ok(privateMessages);
        }

        // PUT api/<PrivateMessagesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PrivateMessagesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
