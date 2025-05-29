using BusinessLogics.Repositories;
using DataAccesses.DTOs.PaginationModels.Users;
using DataAccesses.DTOs.UserFriends;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class UserFriendRepository : GenericRepository<UserFriend>, IUserFriendRepository
    {
        public UserFriendRepository(FakeDiscordContext context) : base(context)
        {
        }

        private IEnumerable<GetUserFriendDTO> GetUserFriendsByStatus(int userId, bool status)
        {
            var friendList = _context.UserFriends.Where(x => x.UserId1 == userId || x.UserId2 == userId).ToList();
            var result = from friend in friendList
                         join user in _context.Users on (friend.UserId1 == userId ? friend.UserId2 : friend.UserId1) equals user.UserId
                         where friend.Status == status && user.IsActive
                         select new GetUserFriendDTO
                         {
                             Id = friend.Id,
                             UserId1 = friend.UserId1,
                             UserId2 = friend.UserId2,
                             Avatar = user.Avatar,
                             UserName = user.UserName,
                             Status = friend.Status,
                             RequestDate = friend.RequestDate
                         };
            return result.AsEnumerable();
        }

        public IEnumerable<GetUserFriendDTO> GetFriendsByUser(int userId)
        {
            return GetUserFriendsByStatus(userId, true);
        }

        public UserFriends GetFriendsByUserPagination(int userId, int page, int itemsPerPage, string? keyword)
        {
            var query = GetUserFriendsByStatus(userId, true);

            // Apply keyword filter before counting/paging for better performance
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query?.Where(f => f.UserName.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            // Materialize the filtered query once
            var filteredFriends = query?.ToList() ?? new List<GetUserFriendDTO>();

            int totalCount = filteredFriends.Count;
            int totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling((double)totalCount / itemsPerPage);
            int skip = Math.Max(0, (page - 1) * itemsPerPage);

            var paginatedData = filteredFriends
                .Skip(skip)
                .Take(itemsPerPage)
                .ToList();

            return new UserFriends
            {
                Data = paginatedData,
                Pages = totalPages
            };
        }
    }
}
