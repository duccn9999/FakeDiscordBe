using BusinessLogics.Repositories;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class AllowedRolesRepository : GenericRepository<AllowedRole>, IAllowedRolesRepository
    {
        public AllowedRolesRepository(FakeDiscordContext context) : base(context)
        {
            
        }
    }
}
