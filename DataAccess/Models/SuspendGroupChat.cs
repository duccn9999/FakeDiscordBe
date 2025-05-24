using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("SuspendGroupChat")]
    public class SuspendGroupChat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int GroupChatId { get; set; }
        public int SuperAdminId { get; set; }
        [ForeignKey("SuperAdminId")]
        public SuperAdmin SuperAdmin { get; set; }
        public string? SuspendReason { get; set; }
        public DateTime DateSuspend { get; set; }
    }
}
