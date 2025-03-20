namespace DataAccesses.DTOs.UserRoles
{
    public class AssignRolesDTO
    {
        public int RoleId { get; set; }
        public List<int> UserIds { get; set; }
    }
}
