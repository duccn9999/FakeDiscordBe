using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAccesses.DTOs.Channels
{
    public class CreatePrivateChannelDTO
    {
        [Required]
        public string ChannelName { get; set; }
        [JsonIgnore]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int UserCreated { get; set; }
        public int GroupChatId { get; set; }
        [JsonIgnore]
        public bool IsPrivate { get; set; } = true;
        public IEnumerable<int> Roles { get; set; }
        public IEnumerable<int> Users { get; set; }
    }
}
