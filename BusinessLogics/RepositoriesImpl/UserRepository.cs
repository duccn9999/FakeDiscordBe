using BusinessLogics.Repositories;
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

        public async Task<bool> CheckUserNameDuplicatedAsync(string userName)
        {
            var user = await table.FirstOrDefaultAsync(x => x.UserName == userName);
            return user != null;
        }

        public User GetByUserName(string userName)
        {
            var user = table.FirstOrDefault(x => x.UserName == userName);
            return user;
        }
    }
}
