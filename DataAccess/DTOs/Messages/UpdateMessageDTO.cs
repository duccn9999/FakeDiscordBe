namespace DataAccesses.DTOs.Messages
{
    public class UpdateMessageDTO
    {
        public int MessageId { get; set; }
        public string Content { get; set; }
        public DateTime? DateModified { get; set; } = DateTime.Now;
        public int ChannelId { get; set; }
    }
}
