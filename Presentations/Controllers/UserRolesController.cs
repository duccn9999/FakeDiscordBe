using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.UserRoles;
using DataAccesses.Models;
using DataAccesses.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = Permissions.CAN_MANAGE_ROLES)]
    public class UserRolesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserRolesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        // POST api/<UserRolesController>
        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRolesDTO model)
        {
            _unitOfWork.BeginTransaction();
            var userRoleDtos = new List<UserRoleDTO>();
            model.UserIds.ForEach(id =>
            {
                userRoleDtos.Add(new UserRoleDTO
                {
                    UserId = id,
                    RoleId = model.RoleId
                });
            });
            var userRoles = _mapper.Map<List<UserRole>>(userRoleDtos);
            _unitOfWork.UserRoles.InsertRange(userRoles);
            _unitOfWork.Commit();
            return NoContent();
        }

        [HttpGet("{groupChatId}")]
        public async Task<IActionResult> GetNumberOfUserByEachRole(int groupChatId)
        {
            var result = _unitOfWork.UserRoles.GetNumberOfUserByEachRole(groupChatId);
            return Ok(result);
        }

        [HttpGet("{groupChatId}/{roleId}")]
        public async Task<IActionResult> GetNumberOfUserByRole(int groupChatId, int roleId)
        {
            var result = await _unitOfWork.UserRoles.GetNumberOfUserByRole(groupChatId, roleId);
            return Ok(result);
        }

        [HttpGet("{groupChatId}/{roleId}")]
        public async Task<IActionResult> GetUsersByEachRole(int groupChatId, int roleId)
        {
            var result = await _unitOfWork.UserRoles.GetUsersByEachRole(groupChatId, roleId);
            return Ok(result);
        }

        [HttpGet("{groupChatId}/{roleId}")]
        public async Task<IActionResult> GetUsersNotInRole(int groupChatId, int roleId)
        {
            var result = await _unitOfWork.UserRoles.GetUsersNotInRole(groupChatId, roleId);
            return Ok(result);
        }
    }
}
