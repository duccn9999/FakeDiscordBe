using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace DataAccesses.DTOs.PrivateMessages
{
    public class CreatePrivateMessageDTO
    {
        public int UserId { get; set; }
        public int Receiver { get; set; }
        public string? Content { get; set; }
        public List<IFormFile>? Images { get; set; }
        [JsonIgnore]
        public DateTime DateCreated = DateTime.Now;
    }
}
