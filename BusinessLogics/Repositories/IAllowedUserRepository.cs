using DataAccesses.DTOs.AllowedRoles;
using DataAccesses.DTOs.AllowedUsers;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IAllowedUserRepository : IGenericRepository<AllowedUser>
    {
        public IEnumerable<int> GetAllowedUsersByChannelId(int channelId);
    }
}
