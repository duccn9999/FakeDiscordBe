using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Presentations.Hubs
{
    [Authorize]
    public class FakeDiscordHub : Hub
    {
        public async Task OnConnected(string username)
        {
            await Clients.Caller.SendAsync("UserConnected", username);
        }

        public async Task GroupChatsDisplay(string str)
        {
            await Clients.Caller.SendAsync("GroupChatsDisplay", str);
        }
    }
}
