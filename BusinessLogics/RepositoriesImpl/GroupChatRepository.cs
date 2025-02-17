using BusinessLogics.Repositories;
using DataAccesses.DTOs.GroupChats;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogics.RepositoriesImpl
{
    public class GroupChatRepository : GenericRepository<GroupChat>, IGroupChatRepository
    {
        private const int MEMBER_ROLE_ID = 1;
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

        public async Task<IAsyncEnumerable<GetGroupChatDTO>> GetJoinedGroupChatPaginationAsync(int userId, int? page, int items)
        {
            var result = from g in _context.GroupChats
                         join p in _context.Participations
                         on g.GroupChatId equals p.GroupChatId
                         join u in _context.Users
                         on p.UserId equals u.UserId
                         where u.UserId == userId && p.RoleId == MEMBER_ROLE_ID
                         select new GetGroupChatDTO
                         {
                             GroupChatId = g.GroupChatId,
                             Name = g.Name,
                             CoverImage = g.CoverImage
                         };
            return result.Skip((page.Value - 1) * items).Take(items).AsAsyncEnumerable();
        }

        public async Task<IAsyncEnumerable<GetGroupChatDTO>> GetJoinedGroupChatsAsync(int userId)
        {
            var result = from g in _context.GroupChats
                         join p in _context.Participations
                         on g.GroupChatId equals p.GroupChatId
                         join u in _context.Users
                         on p.UserId equals u.UserId
                         where u.UserId == userId && p.RoleId == MEMBER_ROLE_ID
                         select new GetGroupChatDTO
                         {
                             GroupChatId = g.GroupChatId,
                             Name = g.Name,
                             CoverImage = g.CoverImage
                         };
            return result.AsAsyncEnumerable();
        }
    }
}
