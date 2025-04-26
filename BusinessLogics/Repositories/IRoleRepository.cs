using DataAccesses.DTOs.Roles;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        public IEnumerable<GetRoleDTO> GetRolesByGroupChatId(int groupChatId);
    }
}
