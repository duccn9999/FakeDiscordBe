using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("BlockedUser")]
    public class BlockedUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId1 { get; set; }
        [ForeignKey("UserId1")]
        public User User1 { get; set; }
        public int UserId2 { get; set; }
        public DateTime BlockedDate { get; set; }
    }
}
