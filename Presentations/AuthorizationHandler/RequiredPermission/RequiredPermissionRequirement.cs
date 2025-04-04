using Microsoft.AspNetCore.Authorization;

namespace Presentations.AuthorizationHandler.RequiredPermission
{
    public class RequiredPermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public RequiredPermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
