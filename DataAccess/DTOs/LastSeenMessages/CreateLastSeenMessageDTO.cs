using System.Text.Json.Serialization;

namespace DataAccesses.DTOs.LastSeenMessages
{
    public class CreateLastSeenMessageDTO
    {
        public int UserId { get; set; }
        public int ChannelId { get; set; }
        public int MessageId { get; set; }
        [JsonIgnore]
        public DateTime DateSeen { get; set; } = DateTime.Now;
    }
}
