﻿using BusinessLogics.Repositories;
using DataAccesses.DTOs.Channels;
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
            var groupChat = _unitOfWork.GroupChats.GetAll().Where(x => x.GroupChatId == channel.GroupChatId).FirstOrDefault();
            // add to channel ( assuming that is a subset of a group )
            await _userTracker.GetUsersByChannel(model.ChannelId, username);
            await Clients.Caller.SendAsync("EnterChannel", username, model);
        }

        public async Task OnLeave(string username, GetChannelDTO model)
        {

            await _userTracker.TrackUsersLeave(model.ChannelId, username);
            await Clients.User(username).SendAsync("UserLeave", username, model);
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
    }
}
