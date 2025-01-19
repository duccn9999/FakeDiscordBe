using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Users;
using DataAccesses.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace BusinessLogics.RepositoriesImpl
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _users;
        public AuthenticationRepository(IUserRepository users, IConfiguration config)
        {
            _users = users;
            _config = config;
        }
        public string GenerateJSONWebToken(LoginUserDTO model)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var user = _users.GetByUserName(model.UserName);
            var claims = new List<Claim> {
                new("userId", user.UserId.ToString()),
                new("username", user.UserName),
                new("email", user.Email),
                new("role", "Member"),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            // need claims
            var token = new JwtSecurityToken(
              null,
              null,
              claims,
              expires: DateTime.Now.AddMinutes(2),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> Login(LoginUserDTO model)
        {
            var IsExisted = await _users.CheckAccoutExistedAsync(model.UserName, model.Password);
            var Result = IsExisted ? GenerateJSONWebToken(model) : null;
            return Result;
        }

        public async Task SignUp(User model)
        {
            var UserFlag = await _users.CheckUserNameDuplicatedAsync(model.UserName);
            var EmailFlag = await _users.CheckEmailDuplicatedAsync(model.Email);
            if (!UserFlag || !EmailFlag)
            {
                _users.Insert(model);
            }
        }
    }
}
