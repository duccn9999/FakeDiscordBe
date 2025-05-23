using DataAccesses.DTOs.Messages;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        public Task<IEnumerable<GetMessageDTO>> GetMessagesByChannelId(int channelId);
        public Task<IEnumerable<GetMessageDTO>> GetMessagesPaginationByChannelId(int channelId, int page, int itemsPerPage);
        public Task<IEnumerable<GetMessageDTO>> GetMessagesPaginationByPrivateChannelId(int channelId);
        public Task<GetTagKeywordDTO> GetTagValue(int groupChatId, string keyword);

    }
}
