namespace DataAccesses.DTOs.PaginationModels
{
    public class UserPaginationDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string? Avatar { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class Users
    {
        public List<UserPaginationDTO> Data { get; set; }
        public int Pages { get; set; }
    }
}
