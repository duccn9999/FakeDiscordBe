using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Channels;
using DataAccesses.DTOs.GroupChatParticipations;
using DataAccesses.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentations.Hubs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly FakeDiscordHub _fakeDiscordHub;
        public ChannelController(IUnitOfWork unitOfWork, IMapper mapper)
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
        [HttpPost("CreateChannel")]
        public async Task<IActionResult> CreateChannel(CreateChannelDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return bad request if the model is invalid
                }
                _unitOfWork.BeginTransaction();
                var Channel = _mapper.Map<Channel>(model);
                _unitOfWork.Channels.Insert(Channel);
                _unitOfWork.Commit();
                return Ok("Create channel success!");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return BadRequest("An internal error occurred."); // Return 500 status code
            }
            finally
            {
                _unitOfWork.Dispose();
            }
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
