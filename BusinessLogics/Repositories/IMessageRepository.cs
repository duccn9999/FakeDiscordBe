using DataAccesses.DTOs.Messages;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        public Task<IAsyncEnumerable<GetMessageDTO>> GetMessagesPaginationByChannelIdAsync(int channelId,  int? page, int items);
    }
}
