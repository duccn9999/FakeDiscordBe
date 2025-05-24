using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("SystemNotification")]
    public class SystemNotification
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SystemNotificationId { get; set; }
        public string Content { get; set; }
        public int UserId {  get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsRead { get; set; }
    }
}
