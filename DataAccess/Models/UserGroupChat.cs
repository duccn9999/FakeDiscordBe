using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Models
{
    [Table("UserGroupChat")]
    public class UserGroupChat
    {
        public int UserId { get; set; }
        public int GroupChatId { get; set; }
        public User? User { get; set; }
        public GroupChat? GroupChat { get; set; }
    }
}
