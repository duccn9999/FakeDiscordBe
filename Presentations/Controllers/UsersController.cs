using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Users;
using DataAccesses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UsersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        // PUT api/<UsersController>/5
        [HttpPut("UpdateProfile/{id}")]
        public async Task<IActionResult> Put(UpdateUserDTO model)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(model.UserId);
            if (user == null)
                return NotFound();
            _unitOfWork.BeginTransaction();
            _mapper.Map(model, user);
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return NoContent();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
