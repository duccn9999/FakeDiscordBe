
using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.GroupChatParticipations;
using DataAccesses.DTOs.GroupChats;
using DataAccesses.Models;
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
        private readonly IHubContext<UserHub> _fakeDiscordHub;
        const int MEMBER_ROLE_ID = 1;
        const int MODERATOR_ROLE_ID = 2;
        public GroupChatsController(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<UserHub> fakeDiscordHub)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fakeDiscordHub = fakeDiscordHub;
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
            var GroupChat = _mapper.Map<GroupChat>(model);
            _unitOfWork.GroupChats.Insert(GroupChat);
            _unitOfWork.Save();
            // add participation
            var participations = new List<Participation>
                {
                    _mapper.Map<Participation>(new AddParticipationDTO
                    {
                        UserId = model.UserCreated,
                        GroupChatId = GroupChat.GroupChatId,
                        RoleId = MODERATOR_ROLE_ID,
                    }),
                    _mapper.Map<Participation>(new AddParticipationDTO
                    {
                        UserId = model.UserCreated,
                        GroupChatId = GroupChat.GroupChatId,
                        RoleId = MEMBER_ROLE_ID,
                    })
                };
            _unitOfWork.Participations.InsertRange(participations);
            _unitOfWork.Commit();
            await _fakeDiscordHub.Clients.User(model.UserCreated.ToString()).SendAsync("GroupChatUpdated", model);
            return Ok("Create group chat success!");
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateGroupChat(UpdateGroupChatDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            var GroupChat = _mapper.Map<GroupChat>(model);
            _unitOfWork.GroupChats.Update(GroupChat);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok("Update group chat success!");
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteGroupChat(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            _unitOfWork.GroupChats.Delete(id);
            _unitOfWork.Commit();
            return Ok("Delete group chat success!");
        }

    }
}
