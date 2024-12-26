
using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.GroupChatParticipations;
using DataAccesses.DTOs.GroupChats;
using DataAccesses.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GroupChatController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet("GetJoinedGroupChats/{userId}")]
        public async Task<IActionResult> GetJoinedGroupChats(int userId)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var result = await _unitOfWork.GroupChats.GetJoinedGroupChatsAsync(userId);
                _unitOfWork.Commit(); // Commit the transaction if everything is successful
                return Ok(result.AsEnumerable());
            }catch(Exception ex) 
            {
                _unitOfWork.Rollback();
                return BadRequest(new { message = "An error occurred while retrieving joined group chats.", error = ex.Message });
            }
            finally
            {
                _unitOfWork.Dispose();
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
                // add user to group chat
                var participation = new AddGroupChatParticipation
                {
                    UserId = model.UserCreated,
                    GroupChatId = GroupChat.GroupChatId,
                    RoleId = 1
                };
                var GroupChatParticipation = _mapper.Map<GroupChatParticipation>(participation);
                _unitOfWork.GroupChatParticipations.Insert(GroupChatParticipation);
                _unitOfWork.Commit();
                return Ok("Create group chat success!");
            }
            catch(Exception ex)
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
                _unitOfWork.Save();
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
