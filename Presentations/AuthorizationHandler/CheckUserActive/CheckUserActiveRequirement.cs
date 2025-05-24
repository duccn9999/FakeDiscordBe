using Microsoft.AspNetCore.Authorization;

namespace Presentations.AuthorizationHandler.IsUserActive
{
    public class CheckUserActiveRequirement : IAuthorizationRequirement
    {
        public CheckUserActiveRequirement()
        {
        }
    }
}
