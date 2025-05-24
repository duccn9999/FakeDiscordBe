using BusinessLogics.Repositories;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class SuspendUserRepository : GenericRepository<SuspendUser>, ISuspendUserRepository
    {
        public SuspendUserRepository(FakeDiscordContext context) : base(context)
        {
            
        }
    }
}
