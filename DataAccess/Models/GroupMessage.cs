using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccesses.Models
{
    [Table("GroupMessage")]
    public class GroupMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupMsgId { get; set; }
        [ForeignKey("UserId")]
        public int UserCreated { get; set; }
        public int? ReplyTo { get; set; }
        [ForeignKey("ReplyTo")]
        public GroupMessage? ReplyToMsg { get; set; }
        [ForeignKey("GroupChatParticipationId")]
        public GroupChatParticipation GroupChatParticipation { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; } 
        public DateTime? DateModified { get; set; }
    }
}
