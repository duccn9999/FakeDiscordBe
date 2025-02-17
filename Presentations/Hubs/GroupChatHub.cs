using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;
using BusinessLogics.Repositories;
using DataAccesses.Models;
using DataAccesses.DTOs.GroupChats;
using DataAccesses.DTOs.Channels;

namespace Presentations.Hubs
{
    [Authorize(Roles = "Member")]
    public class GroupChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserTracker _userTracker;
        public GroupChatHub(IUnitOfWork unitOfWork, UserTracker userTracker)
        {
            _unitOfWork = unitOfWork;
            _userTracker = userTracker;
        }
        public async Task OnEnterGroupChat(string username, GetGroupChatDTO groupChat)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupChat.GroupChatId.ToString());
            await Clients.Group(groupChat.GroupChatId.ToString()).SendAsync("EnterGroupChat", username, groupChat);
        }

        public async Task OnRefreshGroupChats()
        {
            string userId = Context.UserIdentifier;
            await Clients.User(userId).SendAsync("GroupChatsRefresh");
        }

        public async Task OnLeaveGroupChat(string username, GetGroupChatDTO groupChat)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupChat.GroupChatId.ToString());
            await Clients.Group(groupChat.GroupChatId.ToString()).SendAsync("LeaveGroupChat", username, groupChat);
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
