using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Notifications;
using DataAccesses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "CHECK_ACTIVE")]
    public class NotificationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NotificationsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        // GET: api/<NotificationController>
        [HttpGet("{receiverId}")]
        public async Task<IActionResult> GetNotifications(int receiverId)
        {
            var notifications = await _unitOfWork.Notifications.GetNotificationsByReceiverId(receiverId);
            return Ok(notifications);
        }
        // PUT api/<NotificationController>/5
        [HttpPut]
        public async Task<IActionResult> AcceptFriendRequestResponse([FromBody] UpdateNotificationDTO model)
        {
            var notification = _unitOfWork.Notifications.GetById(model.NotificationId);
            if(notification == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            notification.IsRead = model.IsRead;
            // two users has become friend
            var sender = _unitOfWork.Users.GetById(notification.UserId1);
            notification.Message = $"{sender.UserName} and you has become friend";
            notification.Type = false;
            _unitOfWork.Notifications.Update(notification);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(new GetNotificationDTO
            {
                NotificationId = notification.NotificationId,
                UserId1 = notification.UserId1,
                UserId2 = notification.UserId2,
                Message = notification.Message,
                IsRead = notification.IsRead,
                Type = notification.Type,
                DateCreated = notification.DateCreated.ToString("yyyy-MM-dd HH:mm")
            });
        }

        [HttpPut("{notificationId}")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            var notification = _unitOfWork.Notifications.GetById(notificationId);
            if (notification == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            notification.IsRead = true;
            _unitOfWork.Notifications.Update(notification);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(new GetNotificationDTO
            {
                NotificationId = notification.NotificationId,
                UserId1 = notification.UserId1,
                UserId2 = notification.UserId2,
                Message = notification.Message,
                IsRead = notification.IsRead,
                Type = notification.Type,
                DateCreated = notification.DateCreated.ToString("yyyy-MM-dd HH:mm")
            });
        }
        [HttpPut("{receiverId}")]
        public async Task<IActionResult> MarkAllNotificationsAsRead(int receiverId)
        {
            var notifications = _unitOfWork.Notifications.GetAll().Where(x => x.UserId2 == receiverId && !x.IsRead).ToList();
            if (notifications == null || !notifications.Any())
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                _unitOfWork.Notifications.Update(notification);
            }
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(notifications.Select(n => new GetNotificationDTO
            {
                NotificationId = n.NotificationId,
                UserId1 = n.UserId1,
                UserId2 = n.UserId2,
                Message = n.Message,
                IsRead = n.IsRead,
                Type = n.Type,
                DateCreated = n.DateCreated.ToString("yyyy-MM-dd HH:mm")
            }));
        }
    }
}
