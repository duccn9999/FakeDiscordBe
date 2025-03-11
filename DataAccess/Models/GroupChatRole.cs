using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Models
{
    [Table("GroupChatRole")]
    public class GroupChatRole
    {
        public int GroupChatId { get; set; }
        public int RoleId { get; set; }
        public GroupChat? GroupChat { get; set; }
        public Role? Role { get; set; }
    }
}
