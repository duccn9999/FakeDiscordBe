using BusinessLogics.Repositories;
using DataAccesses.DTOs.Notifications;
using DataAccesses.DTOs.PrivateMessages;
using DataAccesses.DTOs.SystemNotifications;
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
        public async Task OnConnected(string userId)
        {
            await Clients.Caller.SendAsync("UserConnected", userId);
        }

        public async Task OnDisconnected(string userId)
        {
            var lastSeenMessage = _userTracker.TrackLastMessage(userId, _unitOfWork).Result;
            await Clients.Caller.SendAsync("UserDisconnected", lastSeenMessage);
        }

        public async Task GetOnlineUsers(int userId)
        {
            await Clients.Caller.SendAsync("GetOnlineUsers");
        }

        public async Task SendFriendRequest(CreateNotificationDTO model)
        {
            var receiver = _unitOfWork.Users.GetById(model.UserId2);
            await Clients.User(receiver.UserId.ToString()).SendAsync("SendFriendRequest", model);
        }

        public async Task AcceptFriendRequest(GetNotificationDTO model)
        {
            var receiver = _unitOfWork.Users.GetById(model.UserId2);
            await Clients.User(receiver.UserId.ToString()).SendAsync("AcceptFriendRequest", model);
        }

        public async Task CancelFriendRequest(GetNotificationDTO model)
        {
            var receiver = _unitOfWork.Users.GetById(model.UserId2);
            await Clients.User(receiver.UserId.ToString()).SendAsync("CancelFriendRequest", model);
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

        public async Task GetSystemNotifications(int userId)
        {
            var user = _unitOfWork.Users.GetById(userId);
            var notifications = _unitOfWork.SystemNotifications.GetAll()
                .Where(x => x.UserId == userId && !x.IsRead)
                .Select(x => new GetSystemNotificationDTO
                {
                    SystemNotificationId = x.SystemNotificationId,
                    Content = x.Content,
                    DateCreated = x.DateCreated.ToString("yyyy-MM-dd HH:mm"),
                    IsRead = x.IsRead,
                    UserId = x.UserId,
                })
                .ToList();
            await Clients.User(user.UserId.ToString()).SendAsync("GetSystemNotifications", notifications);
        }
    }
}
