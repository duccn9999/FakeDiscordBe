using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("UserFriend")]
    public class UserFriend
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /* sender */
        public int UserId1 { get; set; }
        [ForeignKey("UserId1")]
        public User User1 { get; set; }
        // The user with the larger ID
        public int UserId2 { get; set; }
        // 0 - pending, 1 - accepted, 2 - blocked, deleted - not accepted
        public int Status { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
