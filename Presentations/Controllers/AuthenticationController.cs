using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Users;
using DataAccesses.Models;
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
        private readonly IHubContext<UserHub> _fakeDiscordHub;
        public AuthenticationController(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<UserHub> fakeDiscordHub)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fakeDiscordHub = fakeDiscordHub;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return bad request if the model is invalid
                }
                _unitOfWork.BeginTransaction();
                var result = await _unitOfWork.Authentication.Login(model);
                if (result == null) // Check if login failed (null result or another failure indicator)
                {
                    _unitOfWork.Rollback();
                    return Unauthorized("Invalid credentials."); // Return 401 if login fails
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                // Log the exception (example: using a logger)
                // _logger.LogError(ex, "An error occurred while logging in.");
                return BadRequest("An internal error occurred."); // Return 500 status code
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUpAsync(CreateUserDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return bad request if the model is invalid
                }
                if(model.Password != model.RepeatPassword)
                {
                    return BadRequest("The passwords must be the same!, please enter again!");
                }
                _unitOfWork.BeginTransaction();
                var user = _mapper.Map<User>(model);
                await _unitOfWork.Authentication.SignUp(user);
                _unitOfWork.Commit();
                return Ok(new { model.UserName, model.Password });
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return BadRequest("An internal error occurred."); // Return 500 status code
            }
        }

    }
}
