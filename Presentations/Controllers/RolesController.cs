using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Channels;
using DataAccesses.DTOs.Roles;
using DataAccesses.Models;
using DataAccesses.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = Permissions.CAN_MANAGE_ROLES)]
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
        public async Task<IActionResult> Get(int groupChatId)
        {
            var result = _unitOfWork.Roles.GetAll()
            .Where(x => x.GroupChatId == groupChatId && x.RoleName != "ADMINISTRATOR" && x.RoleName != "MEMBER")
            .Select(x => new GetRoleDTO
            {
                RoleId = x.RoleId,
                RoleName = x.RoleName,
                Color = x.Color
            });
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
            return Created("POST", model);
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
            _unitOfWork.BeginTransaction();
            _unitOfWork.Roles.Delete(roleId);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return NoContent();
        }
    }
}
