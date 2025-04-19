using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.PrivateMessageImages;
using DataAccesses.DTOs.PrivateMessages;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogics.RepositoriesImpl
{
    public class PrivateMessageRepository : GenericRepository<PrivateMessage>, IPrivateMessageRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public PrivateMessageRepository(FakeDiscordContext context) : base(context) { }

        public IEnumerable<GetPrivateMessageDTO> GetPrivateMsgesPagination(int page, int? items, int userId, int receiver)
        {
            var result = from privateMessage in _context.PrivateMsgs
                         join user in _context.Users on privateMessage.UserId equals user.UserId
                         where ( privateMessage.UserId == userId && privateMessage.Receiver == receiver ) || 
                         (privateMessage.UserId == receiver && privateMessage.Receiver == userId)
                         select new GetPrivateMessageDTO
                         {
                             MessageId = privateMessage.MessageId,
                             UserId = privateMessage.UserId,
                             UserName = user.UserName,
                             Avatar = user.Avatar,
                             Receiver = privateMessage.Receiver,
                             Content = privateMessage.Content,
                             Images = privateMessage.Images.Select(i => new GetPrivateMessageImageDTO
                             {
                                 ImageId = i.ImageId,
                                 ImageUrl = i.ImageUrl,
                                 MessageId = i.MessageId,
                             }).ToList(),
                             DateCreated = privateMessage.DateCreated.ToString("yyyy-MM-dd HH:mm")
                         };
            return result.AsEnumerable();
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
