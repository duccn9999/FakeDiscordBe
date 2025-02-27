﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccesses.Models
{
    [Table("Message")]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        public int UserCreated { get; set; }
        public int? ReplyTo { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; } 
        public DateTime? DateModified { get; set; }

        public int ChannelId { get; set; }
        [ForeignKey("ChannelId")]
        public Channel Channel { get; set; }
    }
}
