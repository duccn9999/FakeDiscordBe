using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.PrivateMessages
{
    public class UpdatePrivateMessageDTO
    {
        public int PrivateMessageId { get; set; }
        public string? Content { get; set; }
        [JsonIgnore]
        public DateTime DateModified = DateTime.Now;
    }
}
