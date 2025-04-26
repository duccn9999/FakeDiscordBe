namespace DataAccesses.DTOs.PrivateMessageAttachments
{
    public class GetPrivateMessageAttachmentDTO
    {
        public int AttachmentId { get; set; }
        public string Url { get; set; }
        public int MessageId { get; set; }
        public string PublicId { get; set; }
        public string DisplayName { get; set; }
        public string ContentType { get; set; }
        public string DownloadLink { get; set; }
    }
}
