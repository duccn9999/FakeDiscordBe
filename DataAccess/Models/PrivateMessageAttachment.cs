using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("PrivateMessageAttachment")]
    public class PrivateMessageAttachment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentId { get; set; }
        public string Url { get; set; }
        public string ContentType { get; set; }
        public string PublicId { get; set; }
        public string OriginalFilename { get; set; }
        public string DisplayName { get; set; }
        public string DownloadLink { get; set; }
        public int MessageId { get; set; }
        [ForeignKey("MessageId")]
        public PrivateMessage PrivateMessage { get; set; }
    }
}
