using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("PrivateMessageImage")]
    public class PrivateMessageImage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
        public int MessageId { get; set; }
        [ForeignKey("MessageId")]
        public PrivateMessage PrivateMessage { get; set; }
    }
}
