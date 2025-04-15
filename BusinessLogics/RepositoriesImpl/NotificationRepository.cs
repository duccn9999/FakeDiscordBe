using BusinessLogics.Repositories;
using DataAccesses.DTOs.Notifications;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogics.RepositoriesImpl
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(FakeDiscordContext context) :base(context)
        {
            
        }

        public async Task<IEnumerable<GetNotificationDTO>> GetNotificationsByReceiverId(int receiverId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId2 == receiverId)
                .Select(n => new GetNotificationDTO
                {
                    NotificationId = n.NotificationId,
                    UserId1 = n.UserId1,
                    UserId2 = n.UserId2,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    Type = n.Type,
                    DateCreated = n.DateCreated,
                }).Take(15)
                .ToListAsync();
            if(notifications == null || notifications.Count == 0)
                return new List<GetNotificationDTO>();
            return notifications;
        }
    }
}
