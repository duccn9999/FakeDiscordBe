using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Avatar { get; set; }
        public string? CoverImage { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public IEnumerable<PrivateMessage>? PrivateMessages { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
        public IEnumerable<UserGroupChat> UserGroupChats { get; set; }
    }
}
