using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace Presentations.Hubs
{
    [Authorize(Roles = "Member")]
    public class GroupChatHub : Hub
    {
        public async Task OnEnterGroupChat(string userId, string groupChatName)
        {
            await Clients.Group(groupChatName).SendAsync(userId, groupChatName);
        }
    }
}
