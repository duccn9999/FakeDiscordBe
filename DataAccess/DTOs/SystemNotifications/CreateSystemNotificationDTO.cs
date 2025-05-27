using System.Text.Json.Serialization;

namespace DataAccesses.DTOs.SystemNotifications
{
    public class CreateSystemNotificationDTO
    {
        public string Content { get; set; }
        [JsonIgnore]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        [JsonIgnore]
        public bool IsRead { get; set; } = false;
    }
}
