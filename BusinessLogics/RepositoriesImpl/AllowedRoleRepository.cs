using BusinessLogics.Repositories;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class AllowedRoleRepository : GenericRepository<AllowedRole>, IAllowedRoleRepository
    {
        public AllowedRoleRepository(FakeDiscordContext context) : base(context)
        {
            
        }
    }
}
