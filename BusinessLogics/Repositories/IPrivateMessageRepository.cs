using DataAccesses.DTOs.PrivateMessages;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IPrivateMessageRepository : IGenericRepository<PrivateMessage>
    {
        public IEnumerable<GetPrivateMessageDTO> GetPrivateMsges(int userId, int receiver);
        public Task<IEnumerable<GetPrivateMessageDTO>> GetPrivateMsgesPagination(int userId, int receiver, int page, int itemsPerPage);
    }
}
