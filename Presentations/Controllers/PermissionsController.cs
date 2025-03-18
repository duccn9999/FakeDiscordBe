using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Permissions;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PermissionsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        // GET: api/<PermissionsController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = _unitOfWork.Permissions.GetPermissions();
            return Ok(result);
        }
    }
}
