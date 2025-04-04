using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("AllowedUser")]
    public class AllowedUser
    {
        public int ChannelId { get; set; }
        [ForeignKey("ChannelId")]
        public Channel? Channel { get; set; }
        public int UserId { get; set; }
    }
}
