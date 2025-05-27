namespace DataAccesses.DTOs.UserFriends
{
    public class SendFriendRequestDTO
    {
        /* sender */
        public int UserId1 { get; set; }
        public string Receiver { get; set; }
        /* 0 = pending, 1 = accepted, deleted = not accepted */
        public bool Status { get; set; } = false;
        public DateTime RequestDate { get; set; } = DateTime.Now;
    }
}
