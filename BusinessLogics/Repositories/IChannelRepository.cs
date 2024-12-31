using System.Threading.Channels;
using DataAccesses.DTOs.Channels;
using DataAccesses.Models;
using Channel = DataAccesses.Models.Channel;
namespace BusinessLogics.Repositories
{
    public interface IChannelRepository : IGenericRepository<Channel>
    {
        public IEnumerable<GetChannelsDTO> GetChannelsByGroupChatId(int groupChatId);
    }
}
