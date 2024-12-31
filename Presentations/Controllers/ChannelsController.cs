using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Channels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChannelsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/<ChannelsController>
        [HttpGet("{groupChatId}")]
        public async Task<IActionResult> GetChannelsByGroupChatId(int groupChatId)
        {
            try
            {   
                var result = _unitOfWork.Channels.GetChannelsByGroupChatId(groupChatId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while retrieving query.", error = ex.Message });
            }

        }

        // POST api/<ChannelsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ChannelsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ChannelsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
