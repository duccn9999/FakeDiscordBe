using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.GroupChats;
using DataAccesses.DTOs.Channels;
using DataAccesses.DTOs.LastSeenMessages;

namespace Presentations.Hubs
{
    [Authorize]
    public class GroupChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserTracker _userTracker;
        public GroupChatHub(IUnitOfWork unitOfWork, UserTracker userTracker)
        {
            _unitOfWork = unitOfWork;
            _userTracker = userTracker;
        }
        public async Task OnEnterGroupChat(string userId, GetGroupChatDTO groupChat)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupChat.GroupChatId.ToString());
            await Clients.Group(groupChat.GroupChatId.ToString()).SendAsync("EnterGroupChat", userId, groupChat);
        }
        public async Task OnConnected(string userId)
        {
            await Clients.User(userId).SendAsync("OnConnected", userId);
        }
        public async Task OnRefreshGroupChats()
        {
            string userId = Context.UserIdentifier;
            await Clients.User(userId).SendAsync("GroupChatsRefresh");
        }

        public async Task<GetLastSeenMessageDTO> OnLeaveGroupChat(string userId, GetGroupChatDTO groupChat)
        {
            var lastSeenMessage = await _userTracker.TrackLastMessage(userId, _unitOfWork);
            // Notify other users in the group
            await Clients.Group(groupChat.GroupChatId.ToString()).SendAsync("LeaveGroupChat", lastSeenMessage);

            // Remove user from the group
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupChat.GroupChatId.ToString());

            // Return the last seen message to the caller
            return lastSeenMessage;
        }
        public async Task GetConnectedUser(int groupChatId)
        {

        }

        public async Task CreateChannel(GetChannelDTO channel)
        {
            var groupChat = await _unitOfWork.GroupChats.GetGroupChatByChannelIdAsync(channel.ChannelId);
            await Clients.Group(groupChat.GroupChatId.ToString()).SendAsync("CreateChannel", channel);
        }
        public async Task UpdateChannel(GetChannelDTO channel)
        {
            var groupChat = await _unitOfWork.GroupChats.GetGroupChatByChannelIdAsync(channel.ChannelId);
            await Clients.Group(groupChat.GroupChatId.ToString()).SendAsync("UpdateChannel", channel);
        }
        public async Task DeleteChannel(GetChannelDTO channel)
        {
            var groupChat = await _unitOfWork.GroupChats.GetGroupChatByChannelIdAsync(channel.ChannelId);
            await Clients.Group(groupChat.GroupChatId.ToString()).SendAsync("DeleteChannel", channel);
        }
    }
}
