using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Users;
using DataAccesses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentations.Hubs;

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private const string DEFAULT_AVATAR = "https://res.cloudinary.com/dywexvvcy/image/upload/v1743734472/default-avatar-icon-of-social-media-user-vector_nu0k8l.jpg";
        public AuthenticationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            var result = await _unitOfWork.Authentication.Login(model);
            if (result == null) // Check if login failed (null result or another failure indicator)
            {
                return Unauthorized("Invalid credentials."); // Return 401 if login fails
            }
            return Ok(result);
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUpAsync(CreateUserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            if (model.Password != model.RepeatPassword)
            {
                return BadRequest("The passwords must be the same!, please enter again!");
            }
            _unitOfWork.BeginTransaction();
            var user = _mapper.Map<User>(model);
            if(user.Avatar == null)
            {
                user.Avatar = DEFAULT_AVATAR;
            }
            await _unitOfWork.Authentication.SignUp(user);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(new { model.UserName, model.Password });
        }

    }
}
