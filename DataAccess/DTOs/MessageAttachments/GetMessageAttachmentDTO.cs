using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.MessageAttachments
{
    public class GetMessageAttachmentDTO
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
