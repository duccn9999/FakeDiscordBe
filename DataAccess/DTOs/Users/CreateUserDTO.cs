using System.ComponentModel.DataAnnotations;

namespace DataAccesses.DTOs.Users
{
    public class CreateUserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
