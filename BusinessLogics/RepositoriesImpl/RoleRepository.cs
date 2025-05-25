using BusinessLogics.Repositories;
using DataAccesses.DTOs.Roles;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.RepositoriesImpl
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(FakeDiscordContext context) : base(context)
        {
            
        }

        public IEnumerable<GetRoleDTO> GetRolesByGroupChatId(int groupChatId)
        {
            var result = from r in _context.Roles
                         join g in _context.GroupChats
                         on r.GroupChatId equals g.GroupChatId
                         where g.GroupChatId == groupChatId && g.IsActive
                         select new GetRoleDTO
                         {
                             RoleId = r.RoleId,
                             RoleName = r.RoleName,
                             Color = r.Color,
                         };
            return result.AsEnumerable();
        }
    }
}
