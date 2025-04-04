using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Channels;
using DataAccesses.DTOs.GroupChatParticipations;
using DataAccesses.Models;
using DataAccesses.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentations.Hubs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]/[action]")]
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
        [HttpGet("{groupChatId}/{userId}")]
        public async Task<IActionResult> GetChannelsByGroupChatId(int groupChatId, int userId)
        {
            var result = _unitOfWork.Channels.GetChannelsByGroupChatId(groupChatId, userId);
            return Ok(result);
        }
        // POST api/<ChannelsController>
        [HttpPost]
        [Authorize(Policy = Permissions.CAN_MANAGE_CHANNELS)]
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
        [HttpPut]
        [Authorize(Policy = Permissions.CAN_MANAGE_CHANNELS)]
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
        [HttpDelete("{channelId}")]
        [Authorize(Policy = Permissions.CAN_MANAGE_CHANNELS)]
        public async Task<IActionResult> DeleteChannel(int channelId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            _unitOfWork.Channels.Delete(channelId);
            _unitOfWork.Commit();
            return NoContent();
        }
        [HttpPost]
        [Authorize(Policy = Permissions.CAN_MANAGE_CHANNELS)]
        public async Task<IActionResult> CreatePrivateChannel(CreatePrivateChannelDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            var channelDto = new CreateChannelDTO
            {
                ChannelName = model.ChannelName,
                GroupChatId = model.GroupChatId,
                UserCreated = model.UserCreated,
            };
            var channel = _mapper.Map<Channel>(channelDto);
            _unitOfWork.Channels.Insert(channel);
            _unitOfWork.Save();
            var rolesInChannelDto = model.Roles;
            var usersInChannelDto = model.Users;
            var rolesInChannel = rolesInChannelDto
            .Select(roleId => new AllowedRole
            {
                ChannelId = channel.ChannelId,
                RoleId = roleId
            })
            .ToList();
            var usersInChannel = usersInChannelDto.Select(userId => new AllowedUser
            {
                ChannelId = channel.ChannelId,
                UserId = userId,
            }).ToList();
            await _unitOfWork.AllowedRoles.InsertRangeAsync(rolesInChannel);
            await _unitOfWork.AllowedUsers.InsertRangeAsync(usersInChannel);
            _unitOfWork.Commit();
            return Created("CreateChannel", new GetChannelDTO { ChannelId = channel.ChannelId, ChannelName = channel.ChannelName });
        }
    }
}
