using BusinessLogics.Repositories;
using DataAccesses.DTOs.SuperAdmins;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLogics.RepositoriesImpl
{
    public class SuperAdminRepository : GenericRepository<SuperAdmin>, ISuperAdminRepository
    {
        private readonly IConfiguration _config;
        public SuperAdminRepository(IConfiguration config, FakeDiscordContext context) : base(context)
        {
            _config = config;
        }

        public string GenerateJSONWebToken(LoginAdminDTO model)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var admin = table.FirstOrDefault(x => x.Username == model.UserName);
            var claims = new List<Claim> {
                new("superAdminId", admin.SuperAdminId.ToString()),
                new("username", admin.Username),
                new("password", admin.Password),
                new("email", admin.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            // need claims
            var token = new JwtSecurityToken(
              null,
              null,
              claims,
              expires: DateTime.Now.AddHours(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> Login(LoginAdminDTO model)
        {
            var admin = await table.FirstOrDefaultAsync(x => x.Username == model.UserName);
            var Result = admin != null ? GenerateJSONWebToken(model) : null;
            return Result;
        }
    }
}
