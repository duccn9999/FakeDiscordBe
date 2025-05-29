using DataAccesses.DTOs.PaginationModels.Users;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IBlockUserRepository : IGenericRepository<BlockedUser>
    {
        public BlockedUsers GetBlockedUsersPagination(int userId, int page, int itemsPerPage, string? keyword);
    }
}
