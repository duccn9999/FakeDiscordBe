using DataAccesses.DTOs.LastSeenMessages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("LastSeenMessage")]
    public class LastSeenMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LastSeenMessageId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ChannelId { get; set; }
        [Required]
        public int MessageId { get; set; }
        [ForeignKey("MessageId")]
        public Message Message { get; set; }
        [Required]
        public DateTime DateSeen { get; set; }

        public static implicit operator LastSeenMessage(GetLastSeenMessageDTO v)
        {
            throw new NotImplementedException();
        }
    }
}
