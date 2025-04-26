using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("PrivateMessage")]
    public class PrivateMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User Sender { get; set; }
        public int Receiver { get; set; }
        public string? Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public IEnumerable<PrivateMessageAttachment>? Attachments { get; set; }
    }
}
