﻿using BusinessLogics.Repositories;
using DataAccesses.DTOs.Channels;
using DataAccesses.Models;
using System.Linq;
using Channel = DataAccesses.Models.Channel;

namespace BusinessLogics.RepositoriesImpl
{
    public class ChannelRepository : GenericRepository<Channel>, IChannelRepository
    {
        public ChannelRepository(FakeDiscordContext context) : base(context)
        {
        }

        public IEnumerable<int> GetAllowedClaimsByPrivateChannel(int channelId)
        {
            return _context.AllowedRoles
                .Where(ar => ar.ChannelId == channelId)
                .Select(ar => ar.RoleId)
                .Union(
                    _context.AllowedUsers
                    .Where(au => au.ChannelId == channelId)
                    .Select(au => au.UserId)
                )
                .AsEnumerable(); // Executes the query immediately and avoids potential issues
        }
        public IEnumerable<GetChannelDTO> GetChannelsByGroupChatId(int groupChatId, int userId)
        {
            var result = (
                from c in _context.Channels
                where c.GroupChatId == 6 && c.IsPrivate == false
                select new GetChannelDTO
                {
                    ChannelId = c.ChannelId,
                    ChannelName = c.ChannelName
                }
            ).Union(
                from c in _context.Channels
                join au in _context.AllowedUsers on c.ChannelId equals au.ChannelId
                where c.GroupChatId == groupChatId && c.IsPrivate == true && au.UserId == userId
                select new GetChannelDTO
                {
                    ChannelId = c.ChannelId,
                    ChannelName = c.ChannelName
                }
            );
            return result.AsEnumerable();
        }
    }
}
