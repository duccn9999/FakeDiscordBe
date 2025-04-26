using BusinessLogics.Repositories;
using DataAccesses.DTOs.PrivateMessageAttachments;
using DataAccesses.DTOs.PrivateMessages;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class PrivateMessageRepository : GenericRepository<PrivateMessage>, IPrivateMessageRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public PrivateMessageRepository(FakeDiscordContext context) : base(context) { }

        public IEnumerable<GetPrivateMessageDTO> GetPrivateMsges(int userId, int receiver)
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
                             Attachments = privateMessage.Attachments.Select(i => new GetPrivateMessageAttachmentDTO
                             {
                                 AttachmentId = i.AttachmentId,
                                 Url = i.Url,
                                 MessageId = i.MessageId,
                                 ContentType = i.ContentType,
                                 DisplayName = i.DisplayName,
                                 PublicId = i.PublicId,
                                 DownloadLink = i.DownloadLink,
                             }).ToList(),
                             DateCreated = privateMessage.DateCreated.ToString("yyyy-MM-dd HH:mm")
                         };
            return result.AsEnumerable();
        }
    }
}
