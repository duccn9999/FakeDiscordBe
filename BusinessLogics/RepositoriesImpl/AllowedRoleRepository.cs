using BusinessLogics.Repositories;
using DataAccesses.DTOs.AllowedRoles;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class AllowedRoleRepository : GenericRepository<AllowedRole>, IAllowedRoleRepository
    {
        public AllowedRoleRepository(FakeDiscordContext context) : base(context)
        {
            
        }

        public IEnumerable<int> GetAllowedRolesByChannelId(int channelId)
        {
            var result = table
                .Where(ar => ar.ChannelId == channelId).Select(ar => ar.RoleId);
            return result.AsEnumerable();
        }
    }
}
