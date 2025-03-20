using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.UserRoles
{
    public class GetNumberOfUserByEachRoleDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int GroupChatId { get; set; }
        public int Total {  get; set; }
    }
}
