using BusinessLogics.Repositories;
using DataAccesses.DTOs.UserRoles;
using DataAccesses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Channels;

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

            var userRoles = _unitOfWork.UserRoles.GetAllRolesByUser(userId);
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

            // Try to get from route parameters first (applies to all HTTP methods)
            var routeData = httpContext.GetRouteData();
            if (routeData?.Values != null)
            {
                // Check for groupChatId directly
                if (TryParseRouteValue(routeData.Values, "groupChatId", out var parsedGroupChatId))
                {
                    return parsedGroupChatId;
                }

                // Check for roleId and get associated groupChatId
                if (TryParseRouteValue(routeData.Values, "roleId", out var parsedRoleId))
                {
                    return await GetGroupChatIdFromRoleId(parsedRoleId);
                }

                // Check for roleId and get associated groupChatId
                if (TryParseRouteValue(routeData.Values, "channelId", out var parsedChannelId))
                {
                    return await GetGroupChatIdFromChannelId(parsedChannelId);
                }
            }

            // For GET/DELETE requests, check query string
            if (httpContext.Request.Method == HttpMethods.Get || httpContext.Request.Method == HttpMethods.Delete)
            {
                if (httpContext.Request.Query.TryGetValue("groupChatId", out var groupChatIdStr) &&
                    int.TryParse(groupChatIdStr, out var groupChatId))
                {
                    return groupChatId;
                }

                if (httpContext.Request.Query.TryGetValue("channelId", out var channelIdStr) &&
                int.TryParse(groupChatIdStr, out var channelId))
                {
                    return await GetGroupChatIdFromChannelId(channelId);
                }
            }
            // For POST/PUT requests, check form and body
            else if (httpContext.Request.Method == HttpMethods.Post || httpContext.Request.Method == HttpMethods.Put)
            {
                // Check form data
                if (httpContext.Request.HasFormContentType)
                {
                    var form = await httpContext.Request.ReadFormAsync();

                    // Check direct groupChatId 
                    if (TryParseFormValue(form, "groupChatId", out var parsedFormGroupChatId))
                    {
                        return parsedFormGroupChatId;
                    }

                    // Check channelId and get associated groupChatId
                    if (TryParseFormValue(form, "channelId", out var parsedFormChannelId))
                    {
                        return await GetGroupChatIdFromChannelId(parsedFormChannelId);
                    }

                    // Check roleId and get associated groupChatId
                    if (TryParseFormValue(form, "roleId", out var parsedFormRoleId))
                    {
                        return await GetGroupChatIdFromRoleId(parsedFormRoleId);
                    }
                }

                // Check JSON body if content type is application/json
                if (httpContext.Request.ContentType?.Contains("application/json") == true)
                {
                    try
                    {
                        // Read the body only if needed
                        httpContext.Request.EnableBuffering();
                        using var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, leaveOpen: true);
                        var body = await reader.ReadToEndAsync();
                        httpContext.Request.Body.Position = 0; // Reset position

                        if (!string.IsNullOrEmpty(body))
                        {
                            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
                            if (json != null)
                            {
                                // Check direct groupChatId
                                if (TryParseJsonValue(json, "groupChatId", out var parsedBodyId))
                                {
                                    return parsedBodyId;
                                }

                                // Check channelId and get associated groupChatId
                                if (TryParseJsonValue(json, "channelId", out var parsedChannelId))
                                {
                                    return await GetGroupChatIdFromChannelId(parsedChannelId);
                                }

                                // Check roleId and get associated groupChatId
                                if (TryParseJsonValue(json, "roleId", out var parsedRoleId))
                                {
                                    return await GetGroupChatIdFromRoleId(parsedRoleId);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

            return null;
        }

        // Helper methods to reduce redundancy
        private bool TryParseRouteValue(RouteValueDictionary values, string key, out int result)
        {
            result = 0;
            return values[key] is string value && int.TryParse(value, out result);
        }

        private bool TryParseFormValue(IFormCollection form, string key, out int result)
        {
            result = 0;
            return form.TryGetValue(key, out var value) && int.TryParse(value, out result);
        }

        private bool TryParseJsonValue(Dictionary<string, object> json, string key, out int result)
        {
            result = 0;
            return json.TryGetValue(key, out var value) && int.TryParse(value.ToString(), out result);
        }

        private async Task<int?> GetGroupChatIdFromChannelId(int channelId)
        {
            var channel = await _unitOfWork.Channels.GetByIdAsync(channelId);
            return channel?.GroupChatId;
        }

        private async Task<int?> GetGroupChatIdFromRoleId(int roleId)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
            return role?.GroupChatId;
        }




    }
}
