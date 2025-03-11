using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.Roles
{
    public class CreateRoleDTO
    {
        public string RoleName { get; set; }
        public string Color { get; set; }
        [JsonIgnore]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int UserCreated { get; set; }
    }
}
