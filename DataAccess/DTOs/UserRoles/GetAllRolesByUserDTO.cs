using Microsoft.Data.SqlClient.DataClassification;

namespace DataAccesses.DTOs.UserRoles
{
    public class GetAllRolesByUserDTO
    {
        public int GroupChatId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
