using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IPrivateMessageRepository : IGenericRepository<PrivateMessage>
    {
        public IEnumerable<PrivateMessage> GetPrivateMsgesPaginationByUserAndReceiver(int userId, int receiver);
        public IEnumerable<PrivateMessage> GetPrivateMsgesPaginationInSpecificTime(int page, int size, string? keyword, DateTime? startDate, DateTime? endDate, int userId, int receiver);
        public IEnumerable<PrivateMessage> GetPrivateMsgesPagination(int page, int? items, int userId, int receiver);
    }
}
