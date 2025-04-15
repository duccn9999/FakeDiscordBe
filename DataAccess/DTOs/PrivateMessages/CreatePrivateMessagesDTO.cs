using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.PrivateMessages
{
    public class CreatePrivateMessagesDTO
    {
        private int UserId { get; set; }
        public int Receiver { get; set; }
        public List<string> Contents { get; set; }
        public DateTime DateCreated { get; set; } =DateTime.Now;
    }
}
