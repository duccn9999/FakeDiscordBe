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
        public bool Status { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
