using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Models
{
    [Table("UserRole")]
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;

        public User? User { get; set; }
        public Role? Role { get; set; }
    }
}
