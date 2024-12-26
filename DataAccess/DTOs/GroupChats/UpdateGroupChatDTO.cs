using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.GroupChats
{
    public class UpdateGroupChatDTO
    {
        public int GroupChatId { get; set; }
        public int Name { get; set; }
        [JsonIgnore]
        public DateTime? DateModified { get; set; } = DateTime.Now;   
        public int? UserModified { get; set; }
    }
}
