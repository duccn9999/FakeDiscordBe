using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.GroupChatParticipations
{
    public class AddGroupChatParticipation
    {
        public int UserId { get; set; }
        public int GroupChatId { get; set; }
        public int RoleId { get; set; }
        public string? NickName { get; set; }
        public DateTime DateJoined { get; set; } = DateTime.Now;
    }
}
