using BusinessLogics.Repositories;
using DataAccesses.DTOs.Channels;
using DataAccesses.DTOs.LastSeenMessages;
using DataAccesses.DTOs.Messages;
using DataAccesses.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Presentations.Hubs
{
    [Authorize]
    public class ChannelHub : Hub
    {
        private readonly UserTracker _userTracker;
        private readonly IUnitOfWork _unitOfWork;
        public ChannelHub(UserTracker userTracker, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userTracker = userTracker;
        }
        public async Task EnterChannel(string userId, GetChannelDTO_Extend model)
        {
            var channel = _unitOfWork.Channels.GetById(model.ChannelId);
            var groupChat = _unitOfWork.GroupChats.GetAll().FirstOrDefault(x => x.GroupChatId == channel.GroupChatId);
            // add to channel ( assuming that is a subset of a group )
            await _userTracker.GetUsersByChannel(model.ChannelId, userId);
            /* if no previous channel then take the current channel,
             * assuming this is the 1st channel that user enter after login
             */
            var lastSeenMessage = await _userTracker.TrackLastMessage(userId, _unitOfWork, model.ChannelId);
            await Clients.Caller.SendAsync("EnterChannel", userId, model);
        }

        public async Task OnConnected(string userId)
        {
            await Clients.User(userId).SendAsync("OnConnected", userId);
        }

        public async Task<GetLastSeenMessageDTO> OnLeave(string userId, GetChannelDTO model)
        {
            var lastSeenMessage = await _userTracker.TrackLastMessage(userId, _unitOfWork, model.ChannelId);

            await _userTracker.TrackUsersLeave(model.ChannelId, userId);

            // Send to the specific user
            await Clients.User(userId).SendAsync("UserLeave", lastSeenMessage);

            // Return the last seen message to the caller
            return lastSeenMessage;
        }
        public async Task SendMessage(GetMessageDTO model)
        {
            var user = _unitOfWork.Users.GetByUsername(model.Username);

            // Get users blocked by the sender
            var blockedByUser = _unitOfWork.BlockedUsers.GetAll()
                .Where(x => x.UserId1 == user.UserId)
                .Select(x => x.UserId2.ToString())
                .ToList();

            // Get users who blocked the sender
            var usersThatBlockedSender = _unitOfWork.BlockedUsers.GetAll()
                .Where(x => x.UserId2 == user.UserId)
                .Select(x => x.UserId1.ToString())
                .ToList();

            // Combine both lists to get all users who should NOT receive the message
            var allBlockedUsers = blockedByUser.Union(usersThatBlockedSender).ToList();

            // Get all users in the channel
            var usersInChannel = _userTracker.usersByChannel[model.ChannelId];

            // Get users who should receive the message (exclude blocked users)
            var usersToSendMessage = usersInChannel.Except(allBlockedUsers).ToList();

            // Send message to the filtered users
            await Clients.Users(usersToSendMessage).SendAsync("SendMessage", model);
        }

        public async Task UpdateMessage(GetMessageDTO model)
        {
            var user = _unitOfWork.Users.GetByUsername(model.Username);

            // Get users blocked by the sender
            var blockedByUser = _unitOfWork.BlockedUsers.GetAll()
                .Where(x => x.UserId1 == user.UserId)
                .Select(x => x.UserId2.ToString())
                .ToList();

            // Get users who blocked the sender
            var usersThatBlockedSender = _unitOfWork.BlockedUsers.GetAll()
                .Where(x => x.UserId2 == user.UserId)
                .Select(x => x.UserId1.ToString())
                .ToList();

            // Combine both lists to get all users who should NOT receive the update
            var allBlockedUsers = blockedByUser.Union(usersThatBlockedSender).ToList();

            // Get all users in the channel
            var usersInChannel = _userTracker.usersByChannel[model.ChannelId];

            // Get users who should receive the update (exclude blocked users)
            var usersToReceiveUpdate = usersInChannel.Except(allBlockedUsers).ToList();

            // Send update to the filtered users
            await Clients.Users(usersToReceiveUpdate).SendAsync("UpdateMessage", model);
        }

        public async Task DeleteMessage(GetMessageDTO model)
        {
            var user = _unitOfWork.Users.GetByUsername(model.Username);

            // Get users blocked by the sender
            var blockedByUser = _unitOfWork.BlockedUsers.GetAll()
                .Where(x => x.UserId1 == user.UserId)
                .Select(x => x.UserId2.ToString())
                .ToList();

            // Get users who blocked the sender
            var usersThatBlockedSender = _unitOfWork.BlockedUsers.GetAll()
                .Where(x => x.UserId2 == user.UserId)
                .Select(x => x.UserId1.ToString())
                .ToList();

            // Combine both lists to get all users who should NOT receive the delete notification
            var allBlockedUsers = blockedByUser.Union(usersThatBlockedSender).ToList();

            // Get all users in the channel
            var usersInChannel = _userTracker.usersByChannel[model.ChannelId];

            // Get users who should receive the delete notification (exclude blocked users)
            var usersToReceiveDelete = usersInChannel.Except(allBlockedUsers).ToList();

            // Send delete notification to the filtered users
            await Clients.Users(usersToReceiveDelete).SendAsync("DeleteMessage", model);
        }

        public async Task MarkMentionsAsRead(string userId, int channelId)
        {
            await Clients.User(userId).SendAsync("MarkMentionsAsRead", channelId);
        }

        public async Task SetMentionCount(string userId, int channelId, int mentionsCount)
        {
            await Clients.Users(userId).SendAsync("SetMentionCount", new { channelId, mentionsCount });
        }

        public async Task AddMentionCount(List<int> userIds, int channelId)
        {
            var users = userIds.Select(x => x.ToString());
            await Clients.Users(users).SendAsync("AddMentionCount", channelId);
        }
    }
}
