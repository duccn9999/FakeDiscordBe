using BusinessLogics.Repositories;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class LastSeenMessageRepository : GenericRepository<LastSeenMessage>, ILastSeenMessageRepository
    {
        public LastSeenMessageRepository(FakeDiscordContext context) : base(context)
        {
        }
    }
}
