using DataAccesses.DTOs.PrivateMessageImages;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.PrivateMessages
{
    public class GetPrivateMessageDTO
    {
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public int Receiver { get; set; }
        public string? Content { get; set; }
        public List<GetPrivateMessageImageDTO>? Images { get; set; }
        public string DateCreated { get; set; }
    }
}
