using System.Text.Json.Serialization;

namespace DataAccesses.DTOs.Notifications
{
    public class UpdateNotificationDTO
    {
        public int NotificationId { get; set; }
        [JsonIgnore]
        public bool IsRead { get; set; } = true;
    }
}
