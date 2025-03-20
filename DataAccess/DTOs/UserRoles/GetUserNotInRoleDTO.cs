using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.UserRoles
{
    public class GetUserNotInRoleDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string CoverImage { get; set; }
    }
}
