using BusinessLogics.Repositories;
using CloudinaryDotNet.Actions;
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

        public User GetByUsername(string userName)
        {
            var user = table.FirstOrDefault(x => x.UserName == userName);
            return user;
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
                        where g.GroupChatId == groupChatId
                        select new GetUserDTO
                        {
                            UserId = u.UserId,
                            UserName = u.UserName,
                            Avatar = u.Avatar,
                        };
            return users.Distinct().AsEnumerable();
        }
    }
}
