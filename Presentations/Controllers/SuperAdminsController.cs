using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.PaginationModels;
using DataAccesses.DTOs.SuperAdmins;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentations.Controllers
{
    [Route("[action]/[controller]")]
    [ApiController]
    public class SuperAdminsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SuperAdminsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{page}/{itemsPerPage}/{keyword}")]
        public async Task<IActionResult> GetUsers(int page, int itemsPerPage, string? keyword)
        {
            var users = _unitOfWork.Users.GetUsersPagination(page, itemsPerPage, keyword);
            return Ok(users);
        }

        [HttpGet("{page}/{itemsPerPage}/{keyword}")]
        public async Task<IActionResult> GetGroupChats(int page, int itemsPerPage, string? keyword)
        {
            var groupChats = _unitOfWork.GroupChats.GetGroupChatsPagination(page, itemsPerPage, keyword);
            return Ok(groupChats);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginAdminDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = _unitOfWork.SuperAdmins.Login(model);
            return Ok(token);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Suspend(int userId)
        {
            var user = _unitOfWork.Users.GetById(userId);
            if(user == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            user.IsActive = true;
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var responseModel = new UserPaginationDTO
            {
                UserId = userId,
                Avatar = user.Avatar,
                Email = user.Email,
                DateCreated = user.DateCreated,
                Username = user.UserName
            };
            return Ok(responseModel);
        }
    }
}
