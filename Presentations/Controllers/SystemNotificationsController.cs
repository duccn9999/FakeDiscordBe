using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.SystemNotifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentations.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "CHECK_ACTIVE")]
    public class SystemNotificationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public SystemNotificationsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetSystemNotifications(int userId)
        {
            var user = _unitOfWork.Users.GetById(userId);
            var notifications = _unitOfWork.SystemNotifications.GetAll().Where(x => x.UserId == userId).ToList();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(notifications ??= null);
        }

        [HttpPut("{systemNotificationId}")]
        public IActionResult MarkNotificationAsRead(int systemNotificationId)
        {
            var notification = _unitOfWork.SystemNotifications.GetById(systemNotificationId);
            if (notification == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            notification.IsRead = true;
            _unitOfWork.SystemNotifications.Update(notification);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(new GetSystemNotificationDTO
            {
                SystemNotificationId = notification.SystemNotificationId,
                UserId = notification.UserId,
                Content = notification.Content,
                DateCreated = notification.DateCreated.ToString("yyyy-MM-dd HH:mm"),
                IsRead = notification.IsRead
            });
        }

        [HttpPut("{userId}")]
        public IActionResult MarkAllNotificationsAsRead(int userId)
        {
            var notifications = _unitOfWork.SystemNotifications.GetAll().Where(x => x.UserId == userId && !x.IsRead).ToList();
            if (notifications == null || !notifications.Any())
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                _unitOfWork.SystemNotifications.Update(notification);
            }
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(notifications.Select(n => new GetSystemNotificationDTO
            {
                SystemNotificationId = n.SystemNotificationId,
                UserId = n.UserId,
                Content = n.Content,
                DateCreated = n.DateCreated.ToString("yyyy-MM-dd HH:mm"),
                IsRead = n.IsRead
            }));
        }
    }
}
