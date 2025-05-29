using DataAccesses.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.DTOs.BlockedUsers
{
    public class CreateBlockedUserDTO
    {
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public DateTime BlockedDate { get; set; } = DateTime.Now;
    }
}
