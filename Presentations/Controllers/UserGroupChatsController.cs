using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Channels;
using DataAccesses.DTOs.UserGroupChats;
using DataAccesses.DTOs.UserRoles;
using DataAccesses.Models;
using DataAccesses.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserGroupChatsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserGroupChatsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/<UserGroupChatsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/<UserGroupChatsController>
        [HttpPost]
        public async Task<IActionResult> Invite([FromBody] UserGroupChatDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            var UserGroupChat = _mapper.Map<UserGroupChat>(model);
            _unitOfWork.UserGroupChats.Insert(UserGroupChat);
            // Add role with user
            var userRole = new UserRoleDTO
            {
                UserId = model.UserId,
                RoleId = (int)RoleSeedEnum.MEMBER_ROLE_ID
            };
            var UserRole = _mapper.Map<UserRole>(model);
            _unitOfWork.UserRoles.Insert(UserRole);
            _unitOfWork.Commit();
            return NoContent();
        }
        // DELETE api/<UserGroupChatsController>/5
        [HttpDelete("{id}")]
        public void Kick(int id)
        {
        }
    }
}
