using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("AllowedRole")]
    public class AllowedRole
    {
        public int ChannelId { get; set; }
        [ForeignKey("ChannelId")]
        public Channel? Channel { get; set; }
        public int RoleId { get; set; }
    }

}
