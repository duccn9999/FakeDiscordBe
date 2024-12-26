using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.GroupChats
{
    public class CreateGroupChatDTO
    {
        [Required]
        public string Name { get; set; }
        public string? CoverImage { get; set; }
        [JsonIgnore]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int UserCreated { get; set; }
    }
}
