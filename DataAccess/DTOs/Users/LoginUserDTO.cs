﻿using System.ComponentModel.DataAnnotations;

namespace DataAccesses.DTOs.Users
{
    public class LoginUserDTO
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
