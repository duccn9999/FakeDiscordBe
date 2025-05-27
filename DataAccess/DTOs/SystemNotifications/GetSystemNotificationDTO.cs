namespace DataAccesses.DTOs.SystemNotifications
{
    public class GetSystemNotificationDTO
    {
        public int SystemNotificationId { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public string DateCreated { get; set; }
        public bool IsRead { get; set; }
    }
}
