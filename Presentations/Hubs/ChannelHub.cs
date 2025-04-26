using BusinessLogics.Repositories;
using DataAccesses.DTOs.Channels;
using DataAccesses.DTOs.Messages;
using DataAccesses.DTOs.Users;
using DataAccesses.Models;
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

        public async Task SendMessageWithTag(GetMessageDTO model)
        {
            var contentList = model.Content.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var currnetGroupChat = await _unitOfWork.GroupChats.GetGroupChatByChannelIdAsync(model.ChannelId);
            var rolesInGroupChat = _unitOfWork.Roles.GetRolesByGroupChatId(currnetGroupChat.GroupChatId);
            var usersInGroupChat = _unitOfWork.Users.GetUsersInGroupChat(currnetGroupChat.GroupChatId);
            List<string> taggedUsers = new List<string>();
            foreach (var item in contentList)
            {
                if (item.StartsWith('@'))
                {
                    // extract the @ to get the keyword
                    var keyword = item.Substring(1);
                    var tagValue = await _unitOfWork.Messages.GetTagValue(currnetGroupChat.GroupChatId, keyword);
                    var taggedRole = rolesInGroupChat.SingleOrDefault(x => x.RoleName == tagValue.Keyword);
                    if(taggedRole == null)
                    {
                        taggedUsers.AddRange(usersInGroupChat.Where(x => x.UserName == tagValue.Keyword).Select(x => x.UserName).ToList());
                    }
                    else
                    {
                        taggedUsers.AddRange(_unitOfWork.Users.GetUsersByRole(taggedRole.RoleId).Select(x => x.UserName));
                    }
                }
            }
            var uniqueTaggedUsers = new HashSet<string>(taggedUsers);
            await Clients.Users(uniqueTaggedUsers).SendAsync("Tag", 1);
        }
    }
}
