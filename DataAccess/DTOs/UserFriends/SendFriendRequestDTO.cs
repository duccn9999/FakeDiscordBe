namespace DataAccesses.DTOs.UserFriends
{
    public class SendFriendRequestDTO
    {
        /* sender */
        public int UserId1 { get; set; }
        public string Receiver { get; set; }
        /* False = not accept yet, True = accepted */
        public bool Status { get; set; } = false;
        public DateTime RequestDate { get; set; } = DateTime.Now;
    }
}
