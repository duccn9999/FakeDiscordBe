
using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.GroupChats;
using DataAccesses.DTOs.RolePermissions;
using DataAccesses.DTOs.Roles;
using DataAccesses.DTOs.UserGroupChats;
using DataAccesses.DTOs.UserRoles;
using DataAccesses.Models;
using DataAccesses.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentations.Hubs;

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupChatsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        private RandomStringGenerator _randomStringGenerator;
        public GroupChatsController(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<UserHub> fakeDiscordHub, ICloudinaryService cloudinaryService, RandomStringGenerator randomStringGenerator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
            _randomStringGenerator = randomStringGenerator;
        }
        [HttpGet("GetJoinedGroupChats/{userId}")]
        public async Task<IActionResult> GetJoinedGroupChats(int userId)
        {
            var result = await _unitOfWork.GroupChats.GetJoinedGroupChatsAsync(userId);
            return Ok(result);
        }
        [HttpGet("GetGroupChatById/{groupChatId}")]
        public async Task<IActionResult> GetGroupChatById(int groupChatId)
        {
            var result = await _unitOfWork.GroupChats.GetGroupChatByIdAsync(groupChatId);
            return Ok(result);
        }
        [HttpGet("GetJoinedGroupChatsPagination/{userId}")]
        public async Task<IActionResult> GetJoinedGroupChatsPagination(int userId, int? page, int items)
        {
            var result = await _unitOfWork.GroupChats.GetJoinedGroupChatPaginationAsync(userId, page, items);
            return Ok(result);
        }
        [HttpPost("Create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateGroupChat([FromForm] CreateGroupChatDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            // upload image to cloudinary
            var uploadedCoverImage = await _cloudinaryService.UploadImage(model.CoverImage);
            var groupChat = _mapper.Map<GroupChat>(model);
            groupChat.CoverImage = uploadedCoverImage;
            groupChat.InviteCode = _randomStringGenerator.GenerateUniqueRandomString(7);
            _unitOfWork.GroupChats.Insert(groupChat);
            _unitOfWork.Save();
            // Add role in group chat
            var roles = new List<Role>()
            {
                _mapper.Map<Role>(new CreateRoleDTO
                {
                RoleName = RolesSeed.ADMINISTRATOR_ROLE,
                Color = "#ffffff",
                UserCreated = model.UserCreated,
                GroupChatId = groupChat.GroupChatId
                }),
                _mapper.Map<Role>(new CreateRoleDTO
                {
                RoleName = RolesSeed.MEMBER_ROLE,
                Color = "#ffffff",
                UserCreated = model.UserCreated,
                GroupChatId = groupChat.GroupChatId
                })
            };
            _unitOfWork.Roles.InsertRange(roles);
            _unitOfWork.Save();
            // Add permissions with role
            var adminRole = _unitOfWork.Roles.GetAll().FirstOrDefault(x => x.RoleName == RolesSeed.ADMINISTRATOR_ROLE && x.GroupChatId == groupChat.GroupChatId);
            var memberRole = _unitOfWork.Roles.GetAll().FirstOrDefault(x => x.RoleName == RolesSeed.MEMBER_ROLE && x.GroupChatId == groupChat.GroupChatId);
            var adminPermissions = new List<RolePermission>()
            {
                _mapper.Map<RolePermission>(new RolePermissionDTO
                {
                    RoleId = adminRole.RoleId,
                    PermissionId = PermissionsId.CAN_MANAGE_CHANNELS_ID
                }),
                _mapper.Map<RolePermission>(new RolePermissionDTO
                {
                    RoleId = adminRole.RoleId,
                    PermissionId = PermissionsId.CAN_MANAGE_ROLES_ID
                }),
                _mapper.Map<RolePermission>(new RolePermissionDTO
                {
                    RoleId = adminRole.RoleId,
                    PermissionId = PermissionsId.CAN_CREATE_INVITES_ID
                }),
                _mapper.Map<RolePermission>(new RolePermissionDTO
                {
                    RoleId = adminRole.RoleId,
                    PermissionId = PermissionsId.CAN_BAN_MEMBERS_ID
                }),
                _mapper.Map<RolePermission>(new RolePermissionDTO
                {
                    RoleId = adminRole.RoleId,
                    PermissionId = PermissionsId.CAN_TIME_OUT_MEMBERS_ID
                }),
                _mapper.Map<RolePermission>(new RolePermissionDTO
                {
                    RoleId = adminRole.RoleId,
                    PermissionId = PermissionsId.CAN_SEND_MESSAGES_ID
                }),
                _mapper.Map<RolePermission>(new RolePermissionDTO
                {
                    RoleId = adminRole.RoleId,
                    PermissionId = PermissionsId.CAN_MANAGE_MESSAGES_ID
                }),
            };
            var memberPermissions = new List<RolePermission>()
            {
                _mapper.Map<RolePermission>(new RolePermissionDTO
                {
                    RoleId = memberRole.RoleId,
                    PermissionId = PermissionsId.CAN_CREATE_INVITES_ID
                }),
                _mapper.Map<RolePermission>(new RolePermissionDTO
                {
                    RoleId = memberRole.RoleId,
                    PermissionId = PermissionsId.CAN_SEND_MESSAGES_ID
                }),
                _mapper.Map<RolePermission>(new RolePermissionDTO
                {
                    RoleId = memberRole.RoleId,
                    PermissionId = PermissionsId.CAN_MANAGE_MESSAGES_ID
                }),
            };
            _unitOfWork.RolePermissions.InsertRange(adminPermissions);
            _unitOfWork.RolePermissions.InsertRange(memberPermissions);
            // Add role with user
            var userRoles = new List<UserRole>
            {
                _mapper.Map<UserRole>(new UserRoleDTO
                {
                    UserId = model.UserCreated,
                    RoleId = adminRole.RoleId
                }),
                _mapper.Map<UserRole>(new UserRoleDTO
                {
                    UserId = model.UserCreated,
                    RoleId = memberRole.RoleId
                })
            };
            _unitOfWork.UserRoles.InsertRange(userRoles);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok("Create group chat success!");
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateGroupChat([FromForm] UpdateGroupChatDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            // upload image to cloudinary
            var groupChat = _unitOfWork.GroupChats.GetById(model.GroupChatId);
            if (model.CoverImage != null)
            {
                var uploadedCoverImage = await _cloudinaryService.UploadImage(model.CoverImage);
                groupChat.CoverImage = uploadedCoverImage;
            }
            _mapper.Map(model, groupChat);
            _unitOfWork.GroupChats.Update(groupChat);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(new GetGroupChatDTO
            {
                GroupChatId = groupChat.GroupChatId,
                CoverImage = groupChat.CoverImage,
                Name = groupChat.Name
            });
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteGroupChat(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            var groupChat = _unitOfWork.GroupChats.GetById(id);
            _unitOfWork.GroupChats.Delete(id);
            var userRoles = _unitOfWork.UserRoles.GetUserRolesByUserId(groupChat.UserCreated);
            _unitOfWork.UserRoles.DeleteRange(userRoles);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok("Delete group chat success!");
        }
        [HttpPost("Invite")]
        public async Task<IActionResult> Invite(UserGroupChatDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            var memberRole = _unitOfWork.Roles.GetAll().FirstOrDefault(x => x.RoleName == RolesSeed.MEMBER_ROLE && x.GroupChatId == model.GroupChatId);
            var userRoleDto = new UserRoleDTO
            {
                UserId = model.UserId,
                RoleId = memberRole.RoleId
            };
            var userRole = _mapper.Map<UserRole>(userRoleDto);
            _unitOfWork.UserRoles.Insert(userRole);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return NoContent();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetInviteCode(int id)
        {
            return Ok(_unitOfWork.GroupChats.GetById(id).InviteCode);
        }
        [HttpGet("[action]/{inviteCode}")]
        public async Task<IActionResult> GetGroupChatByInviteCode(string inviteCode)
        {
            var GroupChat = await _unitOfWork.GroupChats.GetGroupChatByInviteCode(inviteCode);
            var result = _mapper.Map<GetGroupChatDTO>(GroupChat);
            return Ok(result);
        }
    }
}
