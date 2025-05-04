using DataAccesses.DTOs.Roles;

namespace DataAccesses.DTOs.Users
{
    public class GetUserWithRolesDTO : GetUserDTO
    {
        public List<GetRoleDTO> Roles { get; set; }
    }
}
