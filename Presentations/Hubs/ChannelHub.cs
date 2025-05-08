using BusinessLogics.Repositories;
using DataAccesses.DTOs.Channels;
using DataAccesses.DTOs.LastSeenMessages;
using DataAccesses.DTOs.Messages;
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
        public async Task OnConnected(string username, GetChannelDTO model)
        {
            var channel = _unitOfWork.Channels.GetById(model.ChannelId);
            var groupChat = _unitOfWork.GroupChats.GetAll().FirstOrDefault(x => x.GroupChatId == channel.GroupChatId);
            // add to channel ( assuming that is a subset of a group )
            await _userTracker.GetUsersByChannel(model.ChannelId, username);
            /* if no previous channel then take the current channel,
             * assuming this is the 1st channel that user enter after login
             */
            var lastSeenMessage = await _userTracker.TrackLastMessage(username, _unitOfWork, model.ChannelId);
            await Clients.Caller.SendAsync("EnterChannel", username, model);
        }

        public async Task<GetLastSeenMessageDTO> OnLeave(string username, GetChannelDTO model)
        {
            var lastSeenMessage = await _userTracker.TrackLastMessage(username, _unitOfWork, model.ChannelId);

            await _userTracker.TrackUsersLeave(model.ChannelId, username);

            // Send to the specific user
            await Clients.User(username).SendAsync("UserLeave", lastSeenMessage);

            // Return the last seen message to the caller
            return lastSeenMessage;
        }
        public async Task SendMessage(GetMessageDTO model)
        {
            await Clients.Users(_userTracker.usersByChannel[model.ChannelId]).SendAsync("SendMessage", model);
        }

        public async Task UpdateMessage(GetMessageDTO model)
        {
            await Clients.Users(_userTracker.usersByChannel[model.ChannelId]).SendAsync("UpdateMessage", model);
        }

        public async Task DeleteMessage(GetMessageDTO model)
        {
            await Clients.Users(_userTracker.usersByChannel[model.ChannelId]).SendAsync("DeleteMessage", model);
        }

        public async Task MarkMentionsAsRead(string username, int channelId)
        {
            await Clients.User(username).SendAsync("MarkMentionsAsRead", channelId);
        }

        public async Task SetMentionCount(string username,int channelId, int mentionsCount)
        {
            await Clients.User(username).SendAsync("SetMentionCount", new { channelId, mentionsCount});
        }
    }
}
