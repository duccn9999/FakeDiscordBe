using BusinessLogics.Repositories;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class AllowedUserRepository : GenericRepository<AllowedUser>, IAllowedUserRepository
    {
        public AllowedUserRepository(FakeDiscordContext context) : base(context)
        {
        }
    }
}
