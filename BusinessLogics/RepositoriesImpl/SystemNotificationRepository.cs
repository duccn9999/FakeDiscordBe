using BusinessLogics.Repositories;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class SystemNotificationRepository : GenericRepository<SystemNotification>, ISystemNotificationRepository
    {
        public SystemNotificationRepository(FakeDiscordContext context) : base(context)
        {
        }
    }
}
