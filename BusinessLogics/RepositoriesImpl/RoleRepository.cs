using BusinessLogics.Repositories;
using DataAccesses.DTOs.PaginationModels.Roles;
using DataAccesses.DTOs.Roles;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(FakeDiscordContext context) : base(context)
        {

        }

        public IEnumerable<GetRoleDTO> GetRolesByGroupChatId(int groupChatId, string? keyword)
        {
            var result = table
                .Where(x => x.GroupChatId == groupChatId
                           && x.RoleName != "ADMINISTRATOR"
                           && x.RoleName != "MEMBER"
                           && (string.IsNullOrEmpty(keyword) || x.RoleName.ToLower().Contains(keyword.ToLower())))
                .Select(x => new GetRoleDTO
                {
                    RoleId = x.RoleId,
                    RoleName = x.RoleName,
                    Color = x.Color
                });
            return result.AsEnumerable();
        }

        public Roles GetRolesByGroupChatIdPagination(int groupChatId, int page, int itemsPerPage, string? keyword)
        {
            var query = table
                .Where(x => x.GroupChatId == groupChatId
                           && x.RoleName != "ADMINISTRATOR"
                           && x.RoleName != "MEMBER"
                           && (string.IsNullOrEmpty(keyword) || x.RoleName.ToLower().Contains(keyword.ToLower())));

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);

            var result = query
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Select(x => new GetRoleDTO
                {
                    RoleId = x.RoleId,
                    RoleName = x.RoleName,
                    Color = x.Color
                })
                .ToList();
            return new Roles
            {
                Pages = totalPages,
                roleDTO = result // Assuming roleDTO should be a list
            };
        }
    }
}
