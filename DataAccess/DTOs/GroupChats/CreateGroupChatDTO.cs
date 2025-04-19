using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAccesses.DTOs.GroupChats
{
    public class CreateGroupChatDTO
    {
        [Required]
        public string Name { get; set; }
        public IFormFile CoverImage { get; set; }
        [JsonIgnore]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int UserCreated { get; set; }
    }
}
