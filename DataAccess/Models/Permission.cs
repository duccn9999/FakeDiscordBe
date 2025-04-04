using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccesses.Models
{
    [Table("Permission")]
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }
        public string? Description { get; set; }
        public IEnumerable<RolePermission>? RolePermissions { get; set; }
    }
}
