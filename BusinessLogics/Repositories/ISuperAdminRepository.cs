using DataAccesses.DTOs.SuperAdmins;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface ISuperAdminRepository : IGenericRepository<SuperAdmin>
    {
        public string GenerateJSONWebToken(LoginAdminDTO model);
        public Task<string> Login(LoginAdminDTO model);
    }
}
