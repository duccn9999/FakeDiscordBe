using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.Users
{
    public class ParticipateInGroupsDTO
    {
        public int? GroupChatId { get; set; }
        public IEnumerable<int>? RoleId { get; set; }
    }
}
