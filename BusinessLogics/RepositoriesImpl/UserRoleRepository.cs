using BusinessLogics.Repositories;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.RepositoriesImpl
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(FakeDiscordContext context) : base(context)
        {
            
        }

        public List<UserRole> GetUserRolesByUserId(int userId)
        {
            var result = _context.UserRoles.Where(x => x.UserId == userId).ToList();
            return result;
        }
    }
}
