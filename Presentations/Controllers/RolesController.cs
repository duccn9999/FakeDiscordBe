using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Channels;
using DataAccesses.DTOs.Roles;
using DataAccesses.DTOs.UserRoles;
using DataAccesses.Models;
using DataAccesses.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = Permissions.CAN_MANAGE_ROLES), Authorize(Policy = "CHECK_ACTIVE")]
    public class RolesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RolesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        // GET: api/<RolesController>
        [HttpGet("GetRolesByGroupChat/{groupChatId}")]
        [Authorize(Policy = Permissions.CAN_MANAGE_CHANNELS)]
        public async Task<IActionResult> Get(int groupChatId, string? keyword)
        {
            var result = _unitOfWork.Roles.GetRolesByGroupChatId(groupChatId, keyword);
            return Ok(result);
        }

        [HttpGet("[action]/{groupChatId}/{page}/{itemsPerPage}")]
        [Authorize(Policy = Permissions.CAN_MANAGE_CHANNELS)]
        public async Task<IActionResult> GetRolesPagination(int groupChatId, int page, int itemsPerPage, string? keyword)
        {
            var result = _unitOfWork.Roles.GetRolesByGroupChatIdPagination(groupChatId,page,itemsPerPage, keyword);
            return Ok(result);
        }

        // GET api/<RolesController>/5
        [HttpGet("{groupChatId}/{roleId}")]
        public async Task<IActionResult> Get(int roleId, int groupChatId)
        {
            return Ok(_unitOfWork.Roles.GetById(roleId));
        }
        // POST api/<RolesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRoleDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            var role = _mapper.Map<Role>(model);
            _unitOfWork.Roles.Insert(role);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var totalUsers = await _unitOfWork.UserRoles.GetNumberOfUserByRole(model.GroupChatId, role.RoleId);
            var responseModel = new GetNumberOfUserByEachRoleDTO
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Color = role.Color,
                DateCreated = role.DateCreated.ToString("dd/MM/yyyy HH:mm"),
                DateModified = role.DateModified.HasValue ? role.DateModified.Value.ToString("dd/MM/yyyy HH:mm") : null,
                Total = totalUsers.Total
            };
            return Ok(responseModel);
        }

        // PUT api/<RolesController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateRoleDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            var role = await _unitOfWork.Roles.GetByIdAsync(model.RoleId);
            _mapper.Map(model, role);
            _unitOfWork.Roles.Update(role);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var totalUsers = await _unitOfWork.UserRoles.GetNumberOfUserByRole(role.GroupChatId, role.RoleId);
            var responseModel = new GetNumberOfUserByEachRoleDTO
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Color = role.Color,
                DateCreated = role.DateCreated.ToString("dd/MM/yyyy HH:mm"),
                DateModified = role.DateModified.HasValue ? role.DateModified.Value.ToString("dd/MM/yyyy HH:mm") : null,
                Total = totalUsers.Total
            };
            return Ok(role);
        }

        // DELETE api/<RolesController>/5
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> Delete(int roleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            var role = _unitOfWork.Roles.GetById(roleId);
            if(role == null)
            {
                return NotFound();
            }
            var totalUsers = await _unitOfWork.UserRoles.GetNumberOfUserByRole(role.GroupChatId, role.RoleId);
            var responseModel = new GetNumberOfUserByEachRoleDTO
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Color = role.Color,
                DateCreated = role.DateCreated.ToString("dd/MM/yyyy HH:mm"),
                DateModified = role.DateModified.HasValue ? role.DateModified.Value.ToString("dd/MM/yyyy HH:mm") : null,
                Total = totalUsers.Total
            };
            _unitOfWork.BeginTransaction();
            _unitOfWork.Roles.Delete(roleId);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(responseModel);
        }
    }
}
