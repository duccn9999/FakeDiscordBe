using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    public class GroupChatBlackList
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlackListId { get; set; }
        public int GroupChatId { get; set; }
        [ForeignKey("GroupChatId")]
        public GroupChat GroupChat { get; set; }
        public int UserId { get; set; }
        public string? BanReason { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
