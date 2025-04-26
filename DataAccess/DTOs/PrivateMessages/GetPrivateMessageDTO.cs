using DataAccesses.DTOs.PrivateMessageAttachments;

namespace DataAccesses.DTOs.PrivateMessages
{
    public class GetPrivateMessageDTO
    {
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public int Receiver { get; set; }
        public string? Content { get; set; }
        public List<GetPrivateMessageAttachmentDTO>? Attachments { get; set; }
        public string DateCreated { get; set; }
    }
}
