using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Channels;
using DataAccesses.DTOs.Roles;
using DataAccesses.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(_unitOfWork.Roles.GetAll());
        }

        // GET api/<RolesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(_unitOfWork.Roles.GetById(id));
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
            var groupChatRole = new GroupChatRole
            {
                GroupChatId = model.GroupChatId,
                RoleId = role.RoleId
            };
            _unitOfWork.GroupChatRoles.Insert(groupChatRole);
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
            _unitOfWork.Commit();
            return Ok(role);
        }

        // DELETE api/<RolesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            _unitOfWork.Roles.Delete(id);
            _unitOfWork.Commit();
            return NoContent();
        }
    }
}
