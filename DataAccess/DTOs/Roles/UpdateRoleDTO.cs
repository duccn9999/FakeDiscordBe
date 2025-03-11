using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.Roles
{
    public class UpdateRoleDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Color { get; set; }
        [JsonIgnore]
        public DateTime? DateModified { get; set; } = DateTime.Now;
        public int? UserModified { get; set; }
    }
}
