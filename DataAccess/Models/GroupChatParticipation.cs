using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("GroupChatParticipation")]
    public class GroupChatParticipation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupChatParticipationId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int GroupChatId { get; set; }
        [ForeignKey("GroupChatId")]
        public GroupChat GroupChat { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        public string? NickName { get; set; }
        public DateTime DateJoined { get; set; }
    }
}

