
using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.GroupChatParticipations;
using DataAccesses.DTOs.GroupChatRoles;
using DataAccesses.DTOs.GroupChats;
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
    [Authorize(Roles = "Member")]
    public class GroupChatsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        const int MEMBER_ROLE_ID = 1;
        const int MODERATOR_ROLE_ID = 2;
        public GroupChatsController(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<UserHub> fakeDiscordHub, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
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
        public async Task<IActionResult> CreateGroupChat(CreateGroupChatDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            // upload image to cloudinary
            var uploadedCoverImage = await _cloudinaryService.UploadImage(model.CoverImage);
            model.CoverImage = uploadedCoverImage;
            var GroupChat = _mapper.Map<GroupChat>(model);
            _unitOfWork.GroupChats.Insert(GroupChat);
            _unitOfWork.Save();
            // Add role in group chat
            var groupChatRoles = new List<GroupChatRole>
            {
                _mapper.Map<GroupChatRole>(new GroupChatRoleDTO
                {
                    GroupChatId = GroupChat.GroupChatId,
                    RoleId = (int)RoleSeedEnum.MEMBER_ROLE_ID
                }),
                _mapper.Map<GroupChatRole>(new GroupChatRoleDTO
                {
                    GroupChatId = GroupChat.GroupChatId,
                    RoleId = (int)RoleSeedEnum.MODERATOR_ROLE_ID,
                })
            };
            _unitOfWork.GroupChatRoles.InsertRange(groupChatRoles);
            // Add role with user
            var userRoles = new List<UserRole>
            {
                _mapper.Map<UserRole>(new UserRoleDTO
                {
                    UserId = model.UserCreated,
                    RoleId = (int)RoleSeedEnum.MEMBER_ROLE_ID
                }),
                _mapper.Map<UserRole>(new UserRoleDTO
                {
                    UserId = model.UserCreated,
                    RoleId = (int)RoleSeedEnum.MODERATOR_ROLE_ID,
                })
            };
            _unitOfWork.UserRoles.InsertRange(userRoles);
            _unitOfWork.Commit();
            return Ok("Create group chat success!");
        }
        [HttpPut("Update")]
        //[Authorize(Roles = "Moderator")]
        public async Task<IActionResult> UpdateGroupChat(UpdateGroupChatDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            // upload image to cloudinary
            var uploadedCoverImage = await _cloudinaryService.UploadImage(model.CoverImage);
            model.CoverImage = uploadedCoverImage;
            var GroupChat = _mapper.Map<GroupChat>(model);
            _unitOfWork.GroupChats.Update(GroupChat);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(new GetGroupChatDTO
            {
                GroupChatId = GroupChat.GroupChatId,
                CoverImage = GroupChat.CoverImage,
                Name = GroupChat.Name
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
            _unitOfWork.Commit();
            return Ok("Delete group chat success!");
        }

    }
}
