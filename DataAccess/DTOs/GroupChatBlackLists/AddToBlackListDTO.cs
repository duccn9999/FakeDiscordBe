using System.Text.Json.Serialization;

namespace DataAccesses.DTOs.GroupChatBlackLists
{
    public class AddToBlackListDTO
    {
        public int GroupChatId { get; set; }
        public int UserId { get; set; }
        public string? BanReason { get; set; }
        [JsonIgnore]
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
