using DataAccesses.DTOs.RolePermissions;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.Repositories
{
    public interface IRolePermissionRepository : IGenericRepository<RolePermission>
    {
        public void ToggleRolePermission(RolePermission model);
        public IEnumerable<RolePermissionDTO> GetRolePermissionsByRoleId(int roleId);
        public bool HasPermission(List<int> roles, string permission);
        public IEnumerable<string> GetPermissionNameByRoleIds(int userId, int groupChatId);
    }
}
