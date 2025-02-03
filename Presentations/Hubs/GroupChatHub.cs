using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;
using BusinessLogics.Repositories;
using DataAccesses.Models;
using DataAccesses.DTOs.GroupChats;

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

        public async Task GetConnectedUser(int groupChatId)
        {

        }
    }
}
