using DataAccesses.DTOs.AllowedRoles;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IAllowedRoleRepository : IGenericRepository<AllowedRole>
    {
        public IEnumerable<int> GetAllowedRolesByChannelId(int channelId);
    }
}
