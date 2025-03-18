using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccesses.Models
{
    [Table("Permission")]
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<RolePermission>? RolePermissions { get; set; }
    }
}
