using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Presentations.Hubs
{
    [Authorize]
    public class UserHub : Hub
    {
        public async Task OnConnected(string username)
        {
            await Clients.Caller.SendAsync("UserConnected", username);
        }
    }
}
