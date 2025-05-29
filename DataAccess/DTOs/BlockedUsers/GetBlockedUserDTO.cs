using DataAccesses.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.DTOs.BlockedUsers
{
    public class GetBlockedUserDTO
    {
        public int Id { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string BlockedDate { get; set; }
    }
}
