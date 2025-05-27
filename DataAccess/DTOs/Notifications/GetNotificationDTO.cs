using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.Notifications
{
    public class GetNotificationDTO
    {
        public int NotificationId { get; set; }
        /* sender */
        public int UserId1 { get; set; }
        /* receiver */
        public int UserId2 { get; set; }
        public string Message { get; set; }
        /* read and unread */
        public bool IsRead { get; set; }
        /* true: friend request, false: mention */
        public bool Type { get; set; }
        public string DateCreated { get; set; }
    }
}
