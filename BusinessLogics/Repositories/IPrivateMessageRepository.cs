using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IPrivateMessageRepository : IGenericRepository<PrivateMessage>
    {
        public IEnumerable<PrivateMessage> GetPrivateMsgesPagination(int page, int size, string? keyword, DateTime? startDate, DateTime? endDate);
    }
}
