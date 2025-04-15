using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Models
{
    [Table("Notification")]
    public class Notification
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }
        /* sender */
        public int UserId1 { get; set; }
        [ForeignKey("UserId1")]
        public User User1 { get; set; }
        /* receiver */
        public int UserId2 { get; set; }
        public string Message { get; set; }
        /* read and unread */
        public bool IsRead { get; set; }
        public bool Type { get; set; } // 1: friend request, 0: message
        public DateTime DateCreated { get; set; }
    }
}
