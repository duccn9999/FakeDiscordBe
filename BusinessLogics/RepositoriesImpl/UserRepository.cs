using BusinessLogics.Repositories;
using CloudinaryDotNet.Actions;
using DataAccesses.DTOs.PaginationModels;
using DataAccesses.DTOs.Roles;
using DataAccesses.DTOs.Users;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogics.RepositoriesImpl
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(FakeDiscordContext context) : base(context) { }
        public async Task<bool> CheckAccoutExistedAsync(string userName, string password)
        {
            var user = await table.FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);
            return user != null;
        }

        public async Task<bool> CheckEmailDuplicatedAsync(string email)
        {
            var user = await table.FirstOrDefaultAsync(x => x.Email == email);
            return user != null;
        }

        public async Task<bool> CheckUsernameDuplicatedAsync(string userName)
        {
            var user = await table.FirstOrDefaultAsync(x => x.UserName == userName);
            return user != null;
        }

        public IEnumerable<GetBlockedUserDTO> GetBlockedUsers(int groupChatId)
        {
            var result = from gcb in _context.GroupChatBlackLists
                         join u in table
                         on gcb.UserId equals u.UserId
                         select new GetBlockedUserDTO
                         {
                             BlackListId = gcb.BlackListId,
                             UserId = u.UserId,
                             UserName = u.UserName,
                             Avatar = u.Avatar
                         };
            return result.AsEnumerable();
        }

        public User GetByUsername(string userName)
        {
            var user = table.FirstOrDefault(x => x.UserName == userName);
            return user;
        }

        public async Task<Users> GetUsersPagination(int page, int itemsPerPage, string? keyword)
        {
            var query = table.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(user => user.UserName.Contains(keyword) || user.Email.Contains(keyword));
            }

            var totalItems = await query.CountAsync(); // Await the Task<int> to get the actual count
            var users = await query
                .OrderBy(u => u.UserId)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Select(user => new UserPaginationDTO
                {
                    UserId = user.UserId,
                    Username = user.UserName,
                    Avatar = user.Avatar,
                    DateCreated = user.DateCreated,
                    Email = user.Email,
                    IsActive = user.IsActive
                })
                .ToListAsync();

            return new Users
            {
                Data = users,
                Pages = (int)Math.Ceiling((double)totalItems / itemsPerPage),
            };
        }

        public IEnumerable<GetUserDTO> GetUsersByRole(int roleId)
        {
            var users = from u in _context.Users
                        join
                        ur in _context.UserRoles
                        on u.UserId equals ur.UserId
                        join r in _context.Roles
                        on ur.RoleId equals r.RoleId
                        where r.RoleId == roleId
                        select new GetUserDTO
                        {
                            UserId = u.UserId,
                            UserName = u.UserName,
                            Avatar = u.Avatar,
                        };
            return users.AsEnumerable();
        }

        public IEnumerable<GetUserDTO> GetUsersInGroupChat(int groupChatId, int caller)
        {
            var users = from u in _context.Users
                        join
                        ur in _context.UserRoles
                        on u.UserId equals ur.UserId
                        join r in _context.Roles
                        on ur.RoleId equals r.RoleId
                        join g in _context.GroupChats
                        on r.GroupChatId equals g.GroupChatId
                        where g.GroupChatId == groupChatId && u.UserId != caller && u.IsActive && g.IsActive
                        select new GetUserDTO
                        {
                            UserId = u.UserId,
                            UserName = u.UserName,
                            Avatar = u.Avatar,
                        };
            return users.Distinct().AsEnumerable();
        }

        public IEnumerable<GetUserDTO> GetUsersInGroupChat(int groupChatId)
        {
            var users = from u in _context.Users
                        join
                        ur in _context.UserRoles
                        on u.UserId equals ur.UserId
                        join r in _context.Roles
                        on ur.RoleId equals r.RoleId
                        join g in _context.GroupChats
                        on r.GroupChatId equals g.GroupChatId
                        where g.GroupChatId == groupChatId && u.IsActive && g.IsActive
                        select new GetUserDTO
                        {
                            UserId = u.UserId,
                            UserName = u.UserName,
                            Avatar = u.Avatar,
                        };
            return users.Distinct().AsEnumerable();
        }

        public IEnumerable<GetUserWithRolesDTO> GetUsersInGroupChatWithRoles(int groupChatId)
        {
            var usersWithRoles = (from u in _context.Users
                                  join ur in _context.UserRoles on u.UserId equals ur.UserId
                                  join r in _context.Roles on ur.RoleId equals r.RoleId
                                  join g in _context.GroupChats on r.GroupChatId equals g.GroupChatId
                                  where r.GroupChatId == groupChatId && u.IsActive && g.IsActive
                                  select new
                                  {
                                      u.UserId,
                                      u.UserName,
                                      u.Avatar,
                                      r.RoleId,
                                      r.RoleName,
                                      r.Color
                                  })
                                 .AsEnumerable() // move to memory
                                 .GroupBy(x => new { x.UserId, x.UserName, x.Avatar })
                                 .Select(g => new GetUserWithRolesDTO
                                 {
                                     UserId = g.Key.UserId,
                                     UserName = g.Key.UserName,
                                     Avatar = g.Key.Avatar,
                                     Roles = g.Select(x => new GetRoleDTO
                                     {
                                         RoleId = x.RoleId,
                                         RoleName = x.RoleName,
                                         Color = x.Color
                                     }).Distinct().ToList()
                                 })
                                 .ToList();

            return usersWithRoles;
        }
        #region AdminPart

        
        public async Task<int> GetTotalUsers()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetUsersCreatedToday()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Users.CountAsync(u => u.DateCreated.Date == today);
        }
        #endregion
    }
}
