using Microsoft.AspNetCore.Http;

namespace DataAccesses.DTOs.Users
{
    public class ChangeAvatarRequestDTO
    {
        public int UserId { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
