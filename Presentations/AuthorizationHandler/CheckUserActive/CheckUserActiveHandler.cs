using BusinessLogics.Repositories;
using Microsoft.AspNetCore.Authorization;
using Presentations.AuthorizationHandler.IsUserActive;

namespace Presentations.AuthorizationHandler.CheckUserActive
{
    public class CheckUserActiveHandler : AuthorizationHandler<CheckUserActiveRequirement>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CheckUserActiveHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CheckUserActiveRequirement requirement)
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
            var user = _unitOfWork.Users.GetById(userId);
            var isInSuspend = _unitOfWork.SuspendUsers.GetAll().FirstOrDefault(x => x.UserId == userId) != null;
            if (!user.IsActive && !isInSuspend) return;
            context.Succeed(requirement);
        }
    }
}
