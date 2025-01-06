using BusinessLogics.Repositories;
using DataAccesses.DTOs.Channels;
using DataAccesses.Models;
using Channel = DataAccesses.Models.Channel;

namespace BusinessLogics.RepositoriesImpl
{
    public class ChannelRepository : GenericRepository<Channel>, IChannelRepository
    {
        public ChannelRepository(FakeDiscordContext context) : base(context)
        {
        }

        public IEnumerable<GetChannelsDTO> GetChannelsByGroupChatId(int groupChatId)
        {
            var result = from g in _context.GroupChats
                         join c in _context.Channels
                         on g.GroupChatId equals c.GroupChatId
                         where g.GroupChatId == groupChatId
                         select new GetChannelsDTO
                         {
                             ChannelId = c.ChannelId,
                             ChannelName = c.ChannelName,
                         };
            return result.AsEnumerable();
        }
    }
}
