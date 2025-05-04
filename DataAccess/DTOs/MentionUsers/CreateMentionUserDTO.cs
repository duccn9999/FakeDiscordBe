using System.Text.Json.Serialization;

namespace DataAccesses.DTOs.MentionUsers
{
    public class CreateMentionUserDTO
    {
        public int MessageId { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public bool IsRead { get; set; } = false;
    }
}
