namespace DataAccesses.DTOs.UserFriends
{
    public class SendFriendRequestDTO
    {
        /* sender */
        public int UserId1 { get; set; }
        public string Receiver { get; set; }
        /* 0 = pending, 1 = accepted, 2 = blocked, deleted = not accepted */
        public int Status { get; set; } = 0;
        public DateTime RequestDate { get; set; } = DateTime.Now;
    }
}
