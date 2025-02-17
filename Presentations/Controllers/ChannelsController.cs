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
            var result = _unitOfWork.Channels.GetChannelsByGroupChatId(groupChatId);
            return Ok(result);

        }

        // POST api/<ChannelsController>
        [HttpPost("CreateChannel")]
        public async Task<IActionResult> CreateChannel(CreateChannelDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            var Channel = _mapper.Map<Channel>(model);
            _unitOfWork.Channels.Insert(Channel);
            _unitOfWork.Commit();
            return Created("CreateChannel", new GetChannelDTO { ChannelId = Channel.ChannelId, ChannelName = Channel.ChannelName });
        }

        // PUT api/<ChannelsController>/5
        [HttpPut("UpdateChannel")]
        public async Task<IActionResult> UpdateChannel(UpdateChannelDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            var channel = await _unitOfWork.Channels.GetByIdAsync(model.ChannelId);
             _mapper.Map(model, channel);
            _unitOfWork.Channels.Update(channel);
            _unitOfWork.Commit();
            return Ok(new GetChannelDTO
            {
                ChannelId = channel.ChannelId,
                ChannelName = channel.ChannelName,
            });
        }

        // DELETE api/<ChannelsController>/5
        [HttpDelete("DeleteChannel/{id}")]
        public async Task<IActionResult> DeleteChannel(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            _unitOfWork.Channels.Delete(id);
            _unitOfWork.Commit();
            return NoContent();
        }
    }
}
