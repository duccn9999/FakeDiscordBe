using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Messages;
using DataAccesses.Models;
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
        public MessagesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/<MessagesController>
        [HttpGet("GetMessagesPaginationByChannelIdAsync/{channelId}")]
        public async Task<IActionResult> Get(int channelId, int? page, int items)
        {
            var result = await _unitOfWork.Messages.GetMessagesPaginationByChannelIdAsync(channelId, page, items);
            return Ok(result);
        }

        // GET api/<MessagesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MessagesController>
        [HttpPost("CreateMessage")]
        public async Task<IActionResult> Post(CreateMessageDTO model)
        {
            _unitOfWork.BeginTransaction();
            var message = _mapper.Map<Message>(model);
            var insertProcess = _unitOfWork.Messages.InsertAsync(message);
            await insertProcess;
            _unitOfWork.Commit();
            return Created("CreateMessage", model);
        }

        // PUT api/<MessagesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MessagesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
