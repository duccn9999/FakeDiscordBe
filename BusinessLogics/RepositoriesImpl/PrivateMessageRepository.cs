using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class PrivateMessageRepository : GenericRepository<PrivateMessage>, IPrivateMessageRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public PrivateMessageRepository(FakeDiscordContext context) : base(context) { }

        public IEnumerable<PrivateMessage> GetPrivateMsgesPagination(int page, int? items, int userId, int receiver)
        {
            var query = GetPrivateMsgesPaginationByUserAndReceiver(userId, receiver);

            // Pagination
            query = query.Skip((page - 1) * items.Value).Take(items.Value);

            return query.AsEnumerable();
        }

        public IEnumerable<PrivateMessage> GetPrivateMsgesPaginationInSpecificTime(int page, int size, string? keyword, DateTime? startDate, DateTime? endDate, int userId, int receiver)
        {
            var query = GetPrivateMsgesPaginationByUserAndReceiver(userId, receiver);

            // Filtering based on optional keyword
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(m => m.Content.Contains(keyword));
            }

            // Filtering based on optional date range
            if (startDate.HasValue)
            {
                query = query.Where(m => m.DateCreated >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(m => m.DateCreated <= endDate.Value);
            }

            // Pagination
            query = query.Skip((page - 1) * size).Take(size);

            return query.AsEnumerable();
        }

        public IEnumerable<PrivateMessage> GetPrivateMsgesPaginationByUserAndReceiver(int userId, int receiver)
        {
            var result = GetAll().Where(m => m.Sender.UserId == userId && m.Receiver == receiver)
                .OrderByDescending(m => m.DateCreated)
                .ToList();
            return result.AsEnumerable();
        }
    }
}
