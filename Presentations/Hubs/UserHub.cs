using BusinessLogics.Repositories;
using DataAccesses.DTOs.Notifications;
using DataAccesses.DTOs.PrivateMessages;
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

        public async Task OnDisconnected(string username)
        {
            var lastSeenMessage = _userTracker.TrackLastMessage(username, _unitOfWork).Result;
            await Clients.Caller.SendAsync("UserDisconnected", lastSeenMessage);
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

        public async Task SendPrivateMessage(GetPrivateMessageDTO model, int sender, int receiver)
        {
            var users = _unitOfWork.Users.GetAll().Where(x => x.UserId == sender || x.UserId == receiver).Select(x => x.UserName).ToList();
            await Clients.Users(users).SendAsync("SendPrivateMessage", model);
        }

        public async Task UpdatePrivateMessage(GetPrivateMessageDTO model, int sender, int receiver)
        {
            var users = _unitOfWork.Users.GetAll().Where(x => x.UserId == sender || x.UserId == receiver).Select(x => x.UserName).ToList();
            await Clients.Users(users).SendAsync("UpdatePrivateMessage", model);
        }

        public async Task DeletePrivateMessage(GetPrivateMessageDTO model, int sender, int receiver)
        {
            var users = _unitOfWork.Users.GetAll().Where(x => x.UserId == sender || x.UserId == receiver).Select(x => x.UserName).ToList();
            await Clients.Users(users).SendAsync("DeletePrivateMessage", model);
        }

        public async Task DeletePrivateMessageAttachment(GetPrivateMessageDTO model, int sender, int receiver)
        {
            var users = _unitOfWork.Users.GetAll().Where(x => x.UserId == sender || x.UserId == receiver).Select(x => x.UserName).ToList();
            await Clients.Users(users).SendAsync("DeletePrivateMessageAttachment", model);
        }
    }
}
