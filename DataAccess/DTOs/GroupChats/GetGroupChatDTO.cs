using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.GroupChats
{
    public class GetGroupChatDTO
    {
        public int GroupChatId { get; set; }
        public string Name { get; set; }
        public string CoverImage { get; set; }
    }
}
