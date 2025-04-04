using BusinessLogics.Repositories;
using DataAccesses.DTOs.Permissions;
using DataAccesses.Models;
using System.Security.Cryptography;

namespace BusinessLogics.RepositoriesImpl
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(FakeDiscordContext context) : base(context) { }
        public IEnumerable<PermissionDTO> GetPermissions()
        {
            var result = from p in _context.Permissions
                         select new PermissionDTO
                         {
                             PermissionId = p.PermissionId,
                             Name = p.DisplayName,
                             Description = p.Description,
                         };
            return result.AsEnumerable();
        }
    }
}
