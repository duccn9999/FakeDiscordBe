using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.Users
{
    public class ChangeEmailDTO
    {
        [Required]
        public int userId { get; set; }
        public string email { get; set; }
        [JsonIgnore]
        public DateTime DateModified { get; set; } = DateTime.Now;
    }
}
