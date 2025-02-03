using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.Users
{
    public class UpdateUserDTO
    {
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Avatar {  get; set; }
        [Required]
        public string Email { get; set; }
        [JsonIgnore]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }
}
