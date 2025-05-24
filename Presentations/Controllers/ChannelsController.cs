using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.AllowedRoles;
using DataAccesses.DTOs.AllowedUsers;
using DataAccesses.DTOs.Channels;
using DataAccesses.Models;
using DataAccesses.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "CHECK_ACTIVE")]
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
            _unitOfWork.Save();
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
            _unitOfWork.Save();
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
            _unitOfWork.Save();
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
            var channel = _mapper.Map<Channel>(model);
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
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Created("CreateChannel", new GetChannelDTO { ChannelId = channel.ChannelId, ChannelName = channel.ChannelName });
        }
        [HttpGet("{groupChatId}/{channelId}")]
        [Authorize(Policy = Permissions.CAN_MANAGE_CHANNELS)]
        public async Task<IActionResult> GetAllowedRolesByChannelId(int groupChatId, int channelId)
        {
            var result = _unitOfWork.AllowedRoles.GetAllowedRolesByChannelId(channelId);
            return Ok(result);
        }

        [HttpGet("{groupChatId}/{channelId}")]
        [Authorize(Policy = Permissions.CAN_MANAGE_CHANNELS)]
        public async Task<IActionResult> GetAllowedUsersByChannelId(int groupChatId, int channelId)
        {
            var result = _unitOfWork.AllowedUsers.GetAllowedUsersByChannelId(channelId);
            return Ok(result);
        }
        [HttpPut("{channelId}")]
        public async Task<IActionResult> UpdatePrivateChannelPermissions(int channelId, [FromBody] UpdatePrivateChannelPermissionsDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }

            var channel = await _unitOfWork.Channels.GetByIdAsync(channelId);
            if (channel == null)
            {
                return NotFound(); // Return not found if the channel does not exist
            }

            // Materialize query results by using ToList() to avoid multiple open DataReaders
            var existingAllowedUsers = _unitOfWork.AllowedUsers.GetAll().Where(x => x.ChannelId == channelId).ToList();
            var existingAllowedRoles = _unitOfWork.AllowedRoles.GetAll().Where(x => x.ChannelId == channelId).ToList();

            _unitOfWork.BeginTransaction();

            // Update AllowedUsers - Remove users not in DTO
            foreach (var user in existingAllowedUsers)
            {
                if (!model.AllowedUsers.Contains(user.UserId))
                {
                    _unitOfWork.AllowedUsers.Delete(user.Id);
                }
            }

            // Add new users from DTO
            foreach (var userId in model.AllowedUsers)
            {
                if (!existingAllowedUsers.Any(u => u.UserId == userId))
                {
                    var allowedUserDto = new CreateAllowedUserDTO
                    {
                        ChannelId = channelId,
                        UserId = userId
                    };
                    var allowedUser = _mapper.Map<AllowedUser>(allowedUserDto);
                    _unitOfWork.AllowedUsers.Insert(allowedUser);
                }
            }

            // Update AllowedRoles - Remove roles not in DTO
            foreach (var role in existingAllowedRoles)
            {
                if (!model.AllowedRoles.Contains(role.RoleId))
                {
                    // Fixed: use AllowedRoles repository instead of AllowedUsers
                    _unitOfWork.AllowedRoles.Delete(role.Id);
                }
            }

            // Add new roles from DTO
            foreach (var roleId in model.AllowedRoles)
            {
                if (!existingAllowedRoles.Any(r => r.RoleId == roleId))
                {
                    var allowedRoleDto = new CreateAllowedRoleDTO
                    {
                        ChannelId = channelId,
                        RoleId = roleId
                    };
                    var allowedRole = _mapper.Map<AllowedRole>(allowedRoleDto);
                    _unitOfWork.AllowedRoles.Insert(allowedRole);
                }
            }

            // Save all changes and commit in one go
            _unitOfWork.Save();
            _unitOfWork.Commit();

            return Ok(new
            {
                Message = "Permissions updated successfully",
            });
        }
    }
}
