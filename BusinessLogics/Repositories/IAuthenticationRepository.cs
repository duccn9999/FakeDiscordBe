using DataAccesses.DTOs.Users;
using DataAccesses.Models;
namespace BusinessLogics.Repositories
{
    public interface IAuthenticationRepository
    {
        public string GenerateJSONWebToken(LoginUserDTO model);
        public Task<string> Login(LoginUserDTO model);
        public Task SignUp(User model);
    }
}
