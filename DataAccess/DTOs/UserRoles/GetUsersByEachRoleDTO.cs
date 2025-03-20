namespace DataAccesses.DTOs.UserRoles
{
    public class GetUsersByEachRoleDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int GroupChatId { get; set; }
        public int RoleId { get; set; }
    }
}
