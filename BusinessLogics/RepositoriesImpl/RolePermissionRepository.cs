using BusinessLogics.Repositories;
using DataAccesses.DTOs.RolePermissions;
using DataAccesses.Models;
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
