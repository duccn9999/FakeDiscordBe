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
            var user = _users.GetUserParticipations(model.UserName);
            var claims = new List<Claim> {
                new(JwtRegisteredClaimNames.Iss, user.UserId.ToString()),
                new(JwtRegisteredClaimNames.Sub, user.UserName),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            if (user.Participations != null)
            {
                var participationsJson = System.Text.Json.JsonSerializer.Serialize(user.Participations);
                claims.Add(new Claim("GroupRoles", participationsJson));
            }
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
