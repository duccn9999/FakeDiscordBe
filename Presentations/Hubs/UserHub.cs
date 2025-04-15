using BusinessLogics.Repositories;
using DataAccesses.DTOs.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Presentations.Hubs
{
    [Authorize]
    public class UserHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserTracker _userTracker;
        public UserHub(IUnitOfWork unitOfWork, UserTracker userTracker)
        {
            _unitOfWork = unitOfWork;
            _userTracker = userTracker;
        }
        public async Task OnConnected(string username)
        {
            await Clients.Caller.SendAsync("UserConnected", username);
        }

        public async Task GetOnlineUsers(int userId)
        {
            await Clients.Caller.SendAsync("GetOnlineUsers");
        }

        public async Task SendFriendRequest(CreateNotificationDTO model)
        {
            var receiver = _unitOfWork.Users.GetById(model.UserId2);
            await Clients.User(receiver.UserName).SendAsync("SendFriendRequest", model);
        }

        public async Task AcceptFriendRequest(GetNotificationDTO model)
        {
            var receiver = _unitOfWork.Users.GetById(model.UserId2);
            await Clients.User(receiver.UserName).SendAsync("AcceptFriendRequest", model);
        }

        public async Task CancelFriendRequest(GetNotificationDTO model)
        {
            var receiver = _unitOfWork.Users.GetById(model.UserId2);
            await Clients.User(receiver.UserName).SendAsync("CancelFriendRequest", model);
        }
    }
}
