using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccesses.Models
{
    [Table("Channel")]
    public class Channel
    {
        [Key]
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int UserCreated {  get; set; }
        public int? UserModified {  get; set; }
        public IEnumerable<Message>? Messages { get; set; }

        public int GroupChatId { get; set; }
        [ForeignKey("GroupChatId")]
        public GroupChat GroupChat { get; set; }
    }
}