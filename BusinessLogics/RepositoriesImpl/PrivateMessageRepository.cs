using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class PrivateMessageRepository : GenericRepository<PrivateMessage>, IPrivateMessageRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public PrivateMessageRepository(FakeDiscordContext context) : base(context) { }
        public PrivateMessageRepository(IMapper mapper) : base(mapper) { }
        public IEnumerable<PrivateMessage> GetPrivateMsgesPagination(int page, int size, string? keyword, DateTime? startDate, DateTime? endDate)
        {
            var query = GetAll();

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
    }
}
