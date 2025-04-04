using BusinessLogics.Repositories;
using DataAccesses.DTOs.UserRoles;
using DataAccesses.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace Presentations.AuthorizationHandler.RequiredPermission
{
    public class RequiredPermissionHandler : AuthorizationHandler<RequiredPermissionRequirement>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RequiredPermissionHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RequiredPermissionRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return;
            }

            var userIdClaim = context.User.FindFirst("userId")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return;
            }

            var groupChatId = await GetGroupChatIdFromRequest(httpContext);
            if (groupChatId == null)
            {
                return;
            }

            var rolesClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(rolesClaim))
            {
                return;
            }

            var userRoles = JsonConvert.DeserializeObject<List<GetAllRolesByUserDTO>>(rolesClaim);
            var rolesInGroup = userRoles
                .Where(r => r.GroupChatId == groupChatId)
                .Select(role => role.RoleId)
                .ToList();

            if (!rolesInGroup.Any())
            {
                return;
            }

            // Check if any of the user's roles have the required permission
            var hasPermission = _unitOfWork.RolePermissions.HasPermission(rolesInGroup, requirement.Permission);

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }

        private async Task<int?> GetGroupChatIdFromRequest(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return null;
            }

            if (httpContext.Request.Method == HttpMethods.Get || httpContext.Request.Method == HttpMethods.Delete)
            {
                // Try to get from route parameters
                var routeData = httpContext.GetRouteData();
                if (routeData?.Values["groupChatId"] is string routeGroupChatId &&
                    int.TryParse(routeGroupChatId, out var parsedGroupChatId))
                {
                    return parsedGroupChatId;
                }
                if (routeData?.Values["roleId"] is string routeRoleId &&
                    int.TryParse(routeRoleId, out var parsedRoleId))
                {
                    return parsedRoleId;
                }
                // Fall back to query parameters
                if (httpContext.Request.Query.TryGetValue("groupChatId", out var groupChatIdStr) &&
                    int.TryParse(groupChatIdStr, out var groupChatId))
                {
                    return groupChatId;
                }
            }
            else if (httpContext.Request.Method == HttpMethods.Post || httpContext.Request.Method == HttpMethods.Put)
            {
                // Enable buffering to read body multiple times
                httpContext.Request.EnableBuffering();

                using var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                httpContext.Request.Body.Position = 0; // Reset stream position

                if (!string.IsNullOrEmpty(body))
                {
                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);

                    if (json.TryGetValue("groupChatId", out var bodyId) && int.TryParse(bodyId.ToString(), out var parsedBodyId))
                    {
                        return parsedBodyId;
                    }

                    if (json.TryGetValue("channelId", out var channelBodyId) && int.TryParse(channelBodyId.ToString(), out var parsedChannelId))
                    {
                        var channel = await _unitOfWork.Channels.GetByIdAsync(parsedChannelId);
                        return channel?.GroupChatId;
                    }

                    if (json.TryGetValue("roleId", out var roleBodyId) && int.TryParse(roleBodyId.ToString(), out var parsedRoleId))
                    {
                        var role = await _unitOfWork.Roles.GetByIdAsync(parsedRoleId);
                        return role?.GroupChatId;
                    }
                }
            }

            return null;
        }




    }
}
