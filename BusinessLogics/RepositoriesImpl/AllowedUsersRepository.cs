using BusinessLogics.Repositories;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class AllowedUsersRepository : GenericRepository<AllowedUser>, IAllowedUsersRepository
    {
        public AllowedUsersRepository(FakeDiscordContext context) : base(context)
        {
        }
    }
}
