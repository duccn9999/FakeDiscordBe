using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("MentionUser")]
    public class MentionUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MessageId { get; set; }
        [ForeignKey("MessageId")]
        public Message Message { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; }
    }
}
