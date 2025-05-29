using DataAccesses.DTOs.UserFriends;

namespace DataAccesses.DTOs.PaginationModels.Users
{
    public class UserFriends
    {
        public List<GetUserFriendDTO> Data { get; set; }
        public int Pages { get; set; }
    }
}
