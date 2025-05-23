using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.EmailTokens
{
    public class CreateEmailTokenDTO
    {
        public int UserId { get; set; }
        public string Token {  get; set; }
    }
}
