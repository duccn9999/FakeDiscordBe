using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.Messages
{
    public class GetMessageDTO
    {
        public string Username { get; set; }
        public string Avatar { get; set; }
        public int? ReplyTo { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; }
        public int ChannelId { get; set; }
    }
}
