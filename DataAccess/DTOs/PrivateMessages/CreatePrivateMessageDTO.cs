using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.PrivateMessages
{
    public class CreatePrivateMessageDTO
    {
        private int UserId { get; set; }
        public int Receiver { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
