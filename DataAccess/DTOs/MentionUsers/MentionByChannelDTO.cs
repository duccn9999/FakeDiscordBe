namespace DataAccesses.DTOs.MentionUsers
{
    public class MentionByChannelDTO
    {
        public int ChannelId { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; }
        public int TotalMentions { get; set; }
    }
}
