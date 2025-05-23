using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.Users
{
    public class ChangeAvatarDTO
    {
        public int UserId { get; set; }
        public string Avatar { get; set; }
    }
}
