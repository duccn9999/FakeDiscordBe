using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAccesses.DTOs.Users
{
    public class ChangeUsernameDTO
    {
        [Required]
        public int userId {  get; set; }
        public string username { get; set; }
        [JsonIgnore]
        public DateTime DateModified { get; set; } = DateTime.Now;
    }
}
