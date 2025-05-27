namespace DataAccesses.DTOs.SystemNotifications
{
    public class CreateSystemNotificationModel
    {
        public string Content { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
    }
}
