using BusinessLogics.RepositoriesImpl;
using DataAccesses.DTOs.PaginationModels;
using DataAccesses.DTOs.Users;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<bool> CheckUsernameDuplicatedAsync(string userName);
        public Task<bool> CheckEmailDuplicatedAsync(string email);
        public Task<bool> CheckAccoutExistedAsync(string userName, string password);
        public User GetByUsername(string userName);
        /* MEMBERS MANAGEMENT */
        public IEnumerable<GetUserDTO> GetUsersInGroupChat(int groupChatId, int caller);
        public IEnumerable<GetUserDTO> GetUsersInGroupChat(int groupChatId);
        public IEnumerable<GetUserWithRolesDTO> GetUsersInGroupChatWithRoles(int groupChatId);
        public IEnumerable<GetUserDTO> GetUsersByRole(int roleId);
        public IEnumerable<GetBlockedUserDTO> GetBlockedUsers(int groupChatId);
        /* ADMIN DASHBOARD*/
        public Task<Users> GetUsersPagination(int page, int itemsPerPage, string? keyword);
        public Task<int> GetTotalUsers();
        public Task<int> GetUserCreatedToday();
    }
}
