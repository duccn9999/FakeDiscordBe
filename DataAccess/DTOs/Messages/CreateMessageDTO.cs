using System.Text.Json.Serialization;

namespace DataAccesses.DTOs.Messages
{
    public class CreateMessageDTO
    {
        public int UserCreated {  get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public int? ReplyTo { get; set; }
        public string Content { get; set; }
        [JsonIgnore]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int ChannelId { get; set; }
    }
}
