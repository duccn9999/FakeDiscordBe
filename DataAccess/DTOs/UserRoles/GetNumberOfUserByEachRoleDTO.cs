namespace DataAccesses.DTOs.UserRoles
{
    public class GetNumberOfUserByEachRoleDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int GroupChatId { get; set; }
        public string Color { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public int Total {  get; set; }
    }
}
