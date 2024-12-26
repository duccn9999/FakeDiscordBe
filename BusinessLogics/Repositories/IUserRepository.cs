using BusinessLogics.RepositoriesImpl;
using DataAccesses.DTOs.Users;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<bool> CheckUserNameDuplicatedAsync(string userName);
        public Task<bool> CheckEmailDuplicatedAsync(string email);
        public Task<bool> CheckAccoutExistedAsync(string userName, string password);
        public UserParticipationDTO GetUserParticipations(string userName);
        public User GetByUserName(string userName);
    }
}
