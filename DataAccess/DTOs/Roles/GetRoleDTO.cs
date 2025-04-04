using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.Roles
{
    public class GetRoleDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Color { get; set; }
    }
}
