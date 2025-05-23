using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("AllowedUser")]
    public class AllowedUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ChannelId { get; set; }
        [ForeignKey("ChannelId")]
        public Channel? Channel { get; set; }
        public int UserId { get; set; }
    }
}
