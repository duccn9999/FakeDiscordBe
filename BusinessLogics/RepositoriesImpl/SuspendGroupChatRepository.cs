using BusinessLogics.Repositories;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class SuspendGroupChatRepository : GenericRepository<SuspendGroupChat>, ISuspendGroupChatRepository
    {
        public SuspendGroupChatRepository(FakeDiscordContext context) : base(context)
        {
            
        }
    }
}
