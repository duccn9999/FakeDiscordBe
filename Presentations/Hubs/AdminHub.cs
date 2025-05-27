using BusinessLogics.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Presentations.Hubs
{
    [Authorize]
    public class AdminHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnConnected(string userId)
        {
            var user = _unitOfWork.Users.GetById(userId);
            await Clients.User(userId).SendAsync("OnConnected", user.UserName);
        }


        public async Task SendNotifications(string notification)
        {
            var convertedUserIds = _unitOfWork.Users.GetAll()
                .Select(u => u.UserId.ToString())
                .ToList();
            if (convertedUserIds.Any())
            {
                await Clients.Users(convertedUserIds).SendAsync("ReceiveNotification", notification);
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveNotification", "No users to notify.");
            }
        }
    }
}
