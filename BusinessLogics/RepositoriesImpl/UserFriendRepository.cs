using BusinessLogics.Repositories;
using DataAccesses.DTOs.UserFriends;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class UserFriendRepository : GenericRepository<UserFriend>, IUserFriendRepository
    {
        public UserFriendRepository(FakeDiscordContext context) : base(context)
        {
        }

        public IEnumerable<GetUserFriendDTO> GetFriendsByUser(int userId)
        {
            var friendList = _context.UserFriends.Where(x => x.UserId1 == userId || x.UserId2 == userId).ToList();
            var result = from friend in friendList
                         join user in _context.Users on (friend.UserId1 == userId ? friend.UserId2 : friend.UserId1) equals user.UserId
                         where friend.Status = true
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
    }
}
