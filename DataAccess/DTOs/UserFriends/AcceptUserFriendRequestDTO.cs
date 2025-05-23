using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.UserFriends
{
    public class AcceptUserFriendRequestDTO
    {
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        [JsonIgnore]
        public int Status { get; set; } = 1;
    }
}
