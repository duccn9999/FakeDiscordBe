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
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int UserCreated { get; set; }
        public int? UserModified { get; set; }
    }
}
