using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.PrivateMessageImages
{
    public class GetPrivateMessageImageDTO
    {
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
        public int MessageId { get; set; }
    }
}
