using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.Users
{
    public class GetUserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string? Avatar { get; set; }
    }
}
