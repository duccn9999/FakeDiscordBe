using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.LastSeenMessages
{
    public class GetLastSeenMessageDTO
    {
        public int UserId { get; set; }
        public int ChannelId { get; set; }
        public int MessageId { get; set; }
        public DateTime DateSeen { get; set; }
    }
}
