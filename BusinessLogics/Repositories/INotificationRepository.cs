using DataAccesses.DTOs.Notifications;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        public Task<IEnumerable<GetNotificationDTO>> GetNotificationsByReceiverId(int receiverId);
    }
}
