using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.UserFriends
{
    public class GetUserFriendDTO
    {
        public int Id { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; }
        public int Status { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
