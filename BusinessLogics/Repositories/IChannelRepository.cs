using DataAccesses.DTOs.Channels;
using DataAccesses.Models;
using Channel = DataAccesses.Models.Channel;
namespace BusinessLogics.Repositories
{
    public interface IChannelRepository : IGenericRepository<Channel>
    {
        public IEnumerable<GetChannelDTO_Extend> GetChannelsByGroupChatId(int groupChatId, int userId);
        public IEnumerable<int> GetAllowedClaimsByPrivateChannel(int channelId);
    }
}
