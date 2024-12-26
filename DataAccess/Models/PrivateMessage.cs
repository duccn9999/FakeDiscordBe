using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("PrivateMessage")]
    public class PrivateMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrivateMsgId { get; set; }
        [ForeignKey("UserId")]
        public User UserCreated { get; set; }
        public int? ReplyTo { get; set; }
        [ForeignKey("ReplyTo")]
        public PrivateMessage? ReplyToMsg { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
