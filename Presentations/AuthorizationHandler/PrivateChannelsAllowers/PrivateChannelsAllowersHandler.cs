using BusinessLogics.Repositories;
using DataAccesses.DTOs.UserRoles;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Presentations.AuthorizationHandler.AllowedIds
{
    public class PrivateChannelsAllowersHandler : AuthorizationHandler<PrivateChannelsAllowersRequirement>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PrivateChannelsAllowersHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PrivateChannelsAllowersRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return;
            }

            if (!int.TryParse(context.User.FindFirst("userId")?.Value, out var userId))
            {
                return;
            }

            var rolesClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(rolesClaim))
            {
                return;
            }
            var channelId = await GetChannelIdFromRequest(httpContext);
            var channel = _unitOfWork.Channels.GetById(channelId);
            // if this is public channel
            if (!channel.IsPrivate)
            {
                context.Succeed(requirement);
                return;
            }
            var userRoles = JsonConvert.DeserializeObject<List<GetAllRolesByUserDTO>>(rolesClaim) ?? new();
            // in private channels, find the list channels that contains claims
            var claims = _unitOfWork.Channels.GetAllowedClaimsByPrivateChannel(channelId.Value).ToHashSet();

            // Check if userId is directly allowed
            if (claims.Contains(userId))
            {
                context.Succeed(requirement);
                return;
            }

            // Check if any roleId is allowed
            foreach (var role in userRoles.Select(r => r.RoleId))
            {
                if (claims.Contains(role))
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }


        private async Task<int?> GetChannelIdFromRequest(HttpContext httpContext)
        {
            // Check if the custom parameter exists in the query string
            if (httpContext.Request.Query.TryGetValue("custom", out var customValue) &&
                int.TryParse(customValue, out var parsedCustomId))
            {
                return parsedCustomId;
            }
            return null;
        }

    }
}
