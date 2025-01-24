using DataAccesses.DTOs.Channels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Presentations.Hubs
{
    [Authorize]
    public class ChannelHub : Hub
    {
        public async Task OnCreated(string channelName)
        {
            await Clients.Caller.SendAsync("ChannelCreated", channelName);
        }
    }
}
