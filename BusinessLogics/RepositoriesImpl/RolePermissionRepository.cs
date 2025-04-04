using BusinessLogics.Repositories;
using DataAccesses.DTOs.RolePermissions;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.RepositoriesImpl
{
    public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(FakeDiscordContext context) : base(context)
        {
            
        }

        public IEnumerable<string> GetPermissionNameByRoleIds(List<int> roleIds)
        {
            var result = from rp in _context.RolePermissions
                         join p in _context.Permissions
                         on rp.PermissionId equals p.PermissionId
                         where roleIds.Contains(rp.RoleId)
                         select rp;
                return result.Select(x => x.Permission.Value).AsEnumerable();
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
