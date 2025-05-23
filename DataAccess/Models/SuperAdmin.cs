using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("SuperAdmin")]
    public class SuperAdmin
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SuperAdminId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
