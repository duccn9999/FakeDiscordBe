using System.Text.Json.Serialization;

namespace DataAccesses.DTOs.UserFriends
{
    public class AcceptUserFriendRequestDTO
    {
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        [JsonIgnore]
        public bool Status { get; set; } = true;
    }
}
