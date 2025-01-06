using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.Channels
{
    public class CreateChannelDTO
    {
        [Required]
        public string ChannelName { get; set; }
        [JsonIgnore]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int UserCreated { get; set; }
        public int GroupChatId { get; set; }
    }
}
