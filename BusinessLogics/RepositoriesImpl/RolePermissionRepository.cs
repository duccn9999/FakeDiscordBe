using BusinessLogics.Repositories;
using DataAccesses.DTOs.RolePermissions;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(FakeDiscordContext context) : base(context)
        {
            
        }

        public IEnumerable<string> GetPermissionNameByRoleIds(int userId, int groupChatId)
        {
            var permissions = (from u in _context.Users
                               join ur in _context.UserRoles on u.UserId equals ur.UserId
                               join r in _context.Roles on ur.RoleId equals r.RoleId
                               join rp in _context.RolePermissions on r.RoleId equals rp.RoleId
                               join p in _context.Permissions on rp.PermissionId equals p.PermissionId
                               where r.GroupChatId == groupChatId && u.UserId == userId
                               select p.Value)
                              .Distinct().AsEnumerable();
            return permissions;
        }

        public IEnumerable<RolePermissionDTO> GetRolePermissionsByRoleId(int roleId)
        {
            var result = from rp in _context.RolePermissions
                         where rp.RoleId == roleId
                         select new RolePermissionDTO
                         {
                             RoleId = rp.RoleId,
                             PermissionId = rp.PermissionId,
                         };
            return result.AsEnumerable();
        }

        public bool HasPermission(List<int> roles, string permission)
        {
            // Get all permissions assigned to the provided roles
            var permissionsOfRoles = _context.RolePermissions
                .Where(rp => roles.Contains(rp.RoleId))
                .Select(rp => rp.Permission.Value) // Assuming there's a navigation property to Permission
                .ToList(); // Materialize the query
            // Check if the required permission exists in the list
            return permissionsOfRoles.Contains(permission);
        }


        public void ToggleRolePermission(RolePermission model)
        {
            var rolePermission = _context.RolePermissions
                .SingleOrDefault(x => x.RoleId == model.RoleId && x.PermissionId == model.PermissionId);

            if (rolePermission != null)
            {
                _context.RolePermissions.Remove(rolePermission); // Use the tracked entity
            }
            else
            {
                _context.RolePermissions.Add(model);
            }
        }

    }
}
