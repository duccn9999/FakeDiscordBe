﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("Role")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Color { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int UserCreated { get; set; }
        public int? UserModified { get; set; }
        [ForeignKey("GroupChatId")]
        public int GroupChatId { get; set; }
        public GroupChat? GroupChat { get; set; }
        public IEnumerable<UserRole>? UserRoles { get; set; }
        public IEnumerable<RolePermission>? RolePermissions { get; set; }
    }
}
