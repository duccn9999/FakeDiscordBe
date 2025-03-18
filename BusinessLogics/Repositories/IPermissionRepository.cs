using DataAccesses.DTOs.Permissions;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.Repositories
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        public IEnumerable<PermissionDTO> GetPermissions();
    }
}
