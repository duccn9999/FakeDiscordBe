using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("EmailToken")]
    public class EmailToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string Token { get; set; }
    }
}
