using DataAccesses.DTOs.UserFriends;
using DataAccesses.Models;
namespace BusinessLogics.Repositories
{
    public interface IUserFriendRepository : IGenericRepository<UserFriend>
    {
        public IEnumerable<GetUserFriendDTO> GetFriendsByUser(int userId);
    }
}
