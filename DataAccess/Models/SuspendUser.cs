using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Models
{
    [Table("SuspendUser")]
    public class SuspendUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SuperAdminId { get; set; }
        [ForeignKey("SuperAdminId")]
        public SuperAdmin SuperAdmin { get; set; }
        public string? SuspendReason { get; set; }
        public DateTime DateSuspend { get; set; }
    }
}
