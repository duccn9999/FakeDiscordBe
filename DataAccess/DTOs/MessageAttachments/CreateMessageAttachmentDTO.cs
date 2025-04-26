namespace DataAccesses.DTOs.MessageAttachments
{
    public class CreateMessageAttachmentDTO
    {
        public string Url { get; set; }
        public int MessageId { get; set; }
        public string PublicId { get; set; }
        public string ContentType { get; set; }
        public string DisplayName { get; set; }
        public string OriginalFilename { get; set; }
        public string DownloadLink { get; set; }
    }
}
