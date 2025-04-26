using System.Text.Json.Serialization;

namespace DataAccesses.DTOs.PrivateMessages
{
    public class UpdatePrivateMessageDTO
    {
        public int PrivateMessageId { get; set; }
        public string? Content { get; set; }
        [JsonIgnore]
        public DateTime DateModified = DateTime.Now;
    }
}
