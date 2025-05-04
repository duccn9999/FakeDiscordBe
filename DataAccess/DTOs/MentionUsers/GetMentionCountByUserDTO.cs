using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.MentionUsers
{
    public class GetMentionCountByUserDTO
    {
        public int ChannelId { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; }
        public int TotalMentions { get; set; }
    }
}
