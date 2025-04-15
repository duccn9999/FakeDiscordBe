namespace DataAccesses.DTOs.Notifications
{
    public class CreateNotificationDTO
    {
        public int NotificationId { get; set; }
        /* sender */
        public int UserId1 { get; set; }
        /* receiver */
        public int UserId2 { get; set; }
        public string Message { get; set; }
        /* read and unread */
        public bool IsRead { get; set; } = false;
        /* true: friend request, false: normal notification */
        public bool Type { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
