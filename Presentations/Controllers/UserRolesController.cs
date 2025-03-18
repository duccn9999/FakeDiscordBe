using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> Post(bool isAssigned, int roleId, int userId)
        {
            _unitOfWork.BeginTransaction();
            _unitOfWork.UserRoles.ToggleAssignRole(isAssigned, roleId, userId);
            _unitOfWork.Commit();
            return NoContent();
        }
    }
}
