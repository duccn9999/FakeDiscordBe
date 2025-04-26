using DataAccesses.DTOs.PrivateMessages;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IPrivateMessageRepository : IGenericRepository<PrivateMessage>
    {
        public IEnumerable<GetPrivateMessageDTO> GetPrivateMsges(int userId, int receiver);
    }
}
