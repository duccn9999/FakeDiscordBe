using DataAccesses.DTOs.PaginationModels.Roles;
using DataAccesses.DTOs.Roles;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        public IEnumerable<GetRoleDTO> GetRolesByGroupChatId(int groupChatId, string? keyword);
        public Roles GetRolesByGroupChatIdPagination(int groupChatId, int page, int itemsPerPage, string? keyword);
    }
}
