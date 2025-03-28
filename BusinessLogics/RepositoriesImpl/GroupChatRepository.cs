﻿using BusinessLogics.Repositories;
using DataAccesses.DTOs.GroupChats;
using DataAccesses.DTOs.UserGroupChats;
using DataAccesses.Models;
using DataAccesses.Utils;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogics.RepositoriesImpl
{
    public class GroupChatRepository : GenericRepository<GroupChat>, IGroupChatRepository
    {
        private List<GetGroupChatDTO> GetGroupChat(int userId)
        {
            var result = from g in _context.GroupChats
                         join ugc in _context.UserGroupChats
                         on g.GroupChatId equals ugc.GroupChatId
                         join u in _context.Users
                         on ugc.UserId equals u.UserId
                         where u.UserId == userId
                         select new GetGroupChatDTO
                         {
                             GroupChatId = g.GroupChatId,
                             Name = g.Name,
                             CoverImage = g.CoverImage
                         };
            return result.ToList();
        }
        public GroupChatRepository(FakeDiscordContext context) : base(context)
        {
        }

        public async Task<GroupChat> GetGroupChatByChannelIdAsync(int channelId)
        {
            var channel = await _context.Channels.SingleOrDefaultAsync(x => x.ChannelId == channelId);
            var groupChat = await _context.GroupChats.FindAsync(channel.GroupChatId);
            return groupChat;
        }

        public async Task<GetGroupChatDTO> GetGroupChatByIdAsync(int groupChatId)
        {
            var groupChat = await _context.GroupChats.FindAsync(groupChatId);
            var result = new GetGroupChatDTO
            {
                GroupChatId = groupChat.GroupChatId,
                Name = groupChat.Name,
                CoverImage = groupChat.CoverImage
            };
            return result;
        }

        public async Task<IEnumerable<GetGroupChatDTO>> GetJoinedGroupChatPaginationAsync(int userId, int? page, int items)
        {
            return GetGroupChat(userId).Skip((page.Value - 1) * items)
                               .Take(items)
                               .AsEnumerable();
        }

        public async Task<IEnumerable<GetGroupChatDTO>> GetJoinedGroupChatsAsync(int userId)
        {
            return GetGroupChat(userId).AsEnumerable();
        }

        public async Task<GroupChat> GetGroupChatByInviteCode(string inviteCode)
        {
            var result = await _context.GroupChats.FirstOrDefaultAsync(x => x.InviteCode == inviteCode);
            return result;
        }
    }
}
