using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Emails;
using DataAccesses.DTOs.PaginationModels;
using DataAccesses.DTOs.PaginationModels.GroupChats;
using DataAccesses.DTOs.PaginationModels.Users;
using DataAccesses.DTOs.SuperAdmins;
using DataAccesses.DTOs.SuspendGroupChats;
using DataAccesses.DTOs.SuspendUsers;
using DataAccesses.DTOs.SystemNotifications;
using DataAccesses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentations.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SuperAdminsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SuperAdminsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region Admin
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginAdminDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = await _unitOfWork.SuperAdmins.Login(model);
            return Ok(token);
        }
        [HttpPost]
        public IActionResult IsAdmin([FromBody] LoginAdminDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var admin = _unitOfWork.SuperAdmins.GetAll().FirstOrDefault(x => x.Username == model.UserName && x.Password == model.Password);
            if(admin == null)
            {
                return BadRequest("Wrong password, please try again!!!");
            }
            return Ok(true);
        }
        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] CreateSystemNotificationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.BeginTransaction();
            var userIds = _unitOfWork.Users.GetAll().Select(u => u.UserId).ToList();
            var notificationsDto = userIds.Select(userId => new CreateSystemNotificationModel
            {
                Content = model.Content,
                UserId = userId,
            }).ToList();
            var systemNotification = _mapper.Map<List<SystemNotification>>(notificationsDto);
            _unitOfWork.SystemNotifications.InsertRange(systemNotification);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(model.Content);
        }
        [HttpGet]
        public async Task<IActionResult> GetOverView()
        {
            var totalUsers = await _unitOfWork.Users.GetTotalUsers();
            var usersCreatedToday = await _unitOfWork.Users.GetUsersCreatedToday();
            var totalGroupChats = await _unitOfWork.GroupChats.GetTotalGroupChats();
            var groupChatsCreatedToday = await _unitOfWork.GroupChats.GetGroupChatsCreatedToday();

            //await Task.WhenAll(totalUsersTask, usersCreatedTodayTask, totalGroupChatsTask, groupChatsCreatedTodayTask);

            var result = new
            {
                TotalUsers = totalUsers,
                UsersCreatedToday = usersCreatedToday,
                TotalGroupChats = totalGroupChats,
                GroupChatsCreatedToday = groupChatsCreatedToday
            };

            return Ok(result);
        }
        #endregion
        #region UsersManagement
        [HttpGet("{page}/{itemsPerPage}")]
        public async Task<IActionResult> GetUsers(int page, int itemsPerPage, string? keyword)
        {
            var users = await _unitOfWork.Users.GetUsersPagination(page, itemsPerPage, keyword);
            return Ok(users);
        }
        [HttpPut]
        public async Task<IActionResult> Suspend(CreateSuspendUserDTO model)
        {
            var user = _unitOfWork.Users.GetById(model.UserId);
            var admin = _unitOfWork.SuperAdmins.GetById(model.SuperAdminId);
            if (user == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            
            user.IsActive = false;
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
            // insert suspend record
            var suspend = _mapper.Map<SuspendUser>(model);
            _unitOfWork.SuspendUsers.Insert(suspend);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            // send email to notify user that this user's account has been suspended
            await _unitOfWork.Emails.SendEmail(new SendEmailDTO
            {
                From = admin.Email,
                To = user.Email,
                Subject = "Account Suspension Notification",
                Body = $@"
                    <html>
                        <body style=""font-family: Arial, sans-serif; background-color: #f9f9f9; color: #333; padding: 24px;"">
                            <p>Dear {user.UserName},</p>
                            <p>Your account has been <strong>suspended</strong> by an administrator.</p>
                            <p><strong>Reason:</strong> {model.SuspendReason}</p>
                            <p>If you believe this is a mistake or have any questions, please contact support.</p>
                            <br/>
                            <p>Best regards,<br/>Admin Team</p>
                        </body>
                    </html>"
            });
            var responseModel = new UserPaginationDTO
            {
                UserId = model.UserId,
                Avatar = user.Avatar,
                Email = user.Email,
                DateCreated = user.DateCreated,
                Username = user.UserName,
                IsActive = user.IsActive,
            };
            return Ok(responseModel);
        }
        [HttpPut("{userId}")]
        public async Task<IActionResult> UnSuspendUser(int userId)
        {
            var user = _unitOfWork.Users.GetById(userId);
            var suspend = _unitOfWork.SuspendUsers.GetAll().FirstOrDefault(x => x.UserId == user.UserId);
            if (user == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            user.IsActive = true;
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
            // delete suspend record
            _unitOfWork.SuspendUsers.Delete(suspend.Id);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var responseModel = new UserPaginationDTO
            {
                UserId = userId,
                Avatar = user.Avatar,
                Email = user.Email,
                DateCreated = user.DateCreated,
                Username = user.UserName,
                IsActive = user.IsActive
            };
            return Ok(responseModel);
        }
        #endregion
        #region GroupChatsManagement
        [HttpGet("{page}/{itemsPerPage}")]
        public IActionResult GetGroupChats(int page, int itemsPerPage, string? keyword)
        {
            var groupChats = _unitOfWork.GroupChats.GetGroupChatsPagination(page, itemsPerPage, keyword);
            return Ok(groupChats);
        }

        [HttpPut]
        public async Task<IActionResult> SuspendGroupChat(CreateSuspendGroupChatDTO model)
        {
            var groupChat = _unitOfWork.GroupChats.GetById(model.GroupChatId);
            var admin = _unitOfWork.SuperAdmins.GetById(model.SuperAdminId);
            var owner = _unitOfWork.Users.GetById(groupChat.UserCreated);
            if (groupChat == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            groupChat.IsActive = false;
            _unitOfWork.GroupChats.Update(groupChat);
            _unitOfWork.Save();
            // insert suspend record
            var suspend = _mapper.Map<SuspendGroupChat>(model);
            _unitOfWork.SuspendGroupChats.Insert(suspend);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            await _unitOfWork.Emails.SendEmail(new SendEmailDTO
            {
                From = admin.Email,
                To = owner.Email,
                Subject = "Group Chat Suspension Notification",
                Body = $@"
                <html>
                    <body style=""font-family: Arial, sans-serif; background-color: #f9f9f9; color: #333; padding: 24px;"">
                        <p>Dear {owner.UserName},</p>
                        <p>We would like to inform you that one of your group chats has been <strong>suspended</strong> by an administrator.</p>
                        <p><strong>Reason:</strong> {model.SuspendReason}</p>
                        <p>If you believe this action was taken in error or have any questions, please contact support.</p>
                        <br/>
                        <p>Best regards,<br/>Admin Team</p>
                    </body>
                </html>"
            });
            var responseModel = new GroupChatPaginationDTO
            {
                GroupChatId = groupChat.GroupChatId,
                CoverImage = groupChat.CoverImage,
                DateCreated = groupChat.DateCreated,
                IsActive = groupChat.IsActive,
                Name = groupChat.Name,
                UserCreated = owner.UserName,
            };
            return Ok(responseModel);
        }
        [HttpPut("{groupChatId}")]
        public async Task<IActionResult> UnSuspendGroupChat(int groupChatId)
        {
            var groupChat = _unitOfWork.GroupChats.GetById(groupChatId);
            var owner = _unitOfWork.Users.GetById(groupChat.UserCreated);
            var suspend = _unitOfWork.SuspendGroupChats.GetAll().FirstOrDefault(x => x.GroupChatId == groupChat.GroupChatId);
            if (groupChat == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            groupChat.IsActive = true;
            _unitOfWork.GroupChats.Update(groupChat);
            _unitOfWork.Save();
            // delete suspend record
            _unitOfWork.SuspendGroupChats.Delete(suspend.Id);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var responseModel = new GroupChatPaginationDTO
            {
                Name = groupChat.Name,
                CoverImage = groupChat.CoverImage,
                DateCreated = groupChat.DateCreated,
                GroupChatId = groupChat.GroupChatId,
                UserCreated = owner.UserName,
                IsActive = groupChat.IsActive
            };
            return Ok(responseModel);
        }
        #endregion

    }
}
