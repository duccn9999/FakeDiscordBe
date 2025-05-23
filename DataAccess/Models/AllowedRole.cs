using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("AllowedRole")]
    public class AllowedRole
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ChannelId { get; set; }
        [ForeignKey("ChannelId")]
        public Channel? Channel { get; set; }
        public int RoleId { get; set; }
    }

}
