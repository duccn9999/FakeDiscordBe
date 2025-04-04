using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("GroupChat")]
    public class GroupChat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupChatId { get; set; }
        public string Name { get; set; }
        public string? CoverImage {  get; set; }
        [MaxLength(7)]
        public string InviteCode { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int UserCreated { get; set; }
        public int? UserModified { get; set; }
        public IEnumerable<Channel>? Channels { get; set; }
        public IEnumerable<Role>? Roles { get; set; }
    }
}
