
using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.GroupChatParticipations;
using DataAccesses.DTOs.GroupChats;
using DataAccesses.Models;
using DataAccesses.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CloudinaryService _cloudinary;
        const int MODERATOR_ROLE_ID = 1;
        const int EVERYONE_ROLE_ID = 0;
        public GroupChatController(IUnitOfWork unitOfWork, IMapper mapper, CloudinaryService cloudinary)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }
        [HttpGet("GetJoinedGroupChats/{userId}")]
        public async Task<IActionResult> GetJoinedGroupChats(int userId)
        {
            try
            {
                var result = await _unitOfWork.GroupChats.GetJoinedGroupChatsAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while retrieving joined group chats.", error = ex.Message });
            }
        }
        [HttpGet("GetGroupChatById/{groupChatId}")]
        public async Task<IActionResult> GetGroupChatById(int groupChatId)
        {
            try
            {
                var result = await _unitOfWork.GroupChats.GetGroupChatByIdAsync(groupChatId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while group chats.", error = ex.Message });
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateGroupChat(CreateGroupChatDTO model)
        {
            try
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
                var Participation = _mapper.Map<Participation>(new AddParticipationDTO
                {
                    UserId = model.UserCreated,
                    GroupChatId = GroupChat.GroupChatId,
                    RoleId = MODERATOR_ROLE_ID,
                });
                _unitOfWork.Participations.Insert(Participation);
                _unitOfWork.Commit();
                return Ok("Create group chat success!");
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
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateGroupChat(UpdateGroupChatDTO model)
        {
            try
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
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteGroupChat(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return bad request if the model is invalid
                }
                _unitOfWork.BeginTransaction();
                var GroupChat = _unitOfWork.GroupChats.GetById(id);
                _unitOfWork.GroupChats.Delete(GroupChat);
                _unitOfWork.Commit();
                return Ok("Delete group chat success!");
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

    }
}
