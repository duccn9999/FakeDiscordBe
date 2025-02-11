using BusinessLogics.Repositories;
using DataAccesses.DTOs.Messages;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogics.RepositoriesImpl
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(FakeDiscordContext context) : base(context) { }
        public async Task<IAsyncEnumerable<GetMessageDTO>> GetMessagesPaginationByChannelIdAsync(int channelId, int? page, int items)
        {
            var result = from m in _context.Messages
                         join c in _context.Channels
                         on m.ChannelId equals c.ChannelId
                         join u in _context.Users
                         on m.UserCreated equals u.UserId
                         where c.ChannelId == channelId orderby m.DateCreated descending
                         select new GetMessageDTO
                         {
                             MessageId = m.MessageId,
                             Username = u.UserName,
                             Avatar = u.Avatar,
                             ReplyTo = m.ReplyTo,
                             Content = m.Content,
                             DateCreated = m.DateCreated,
                             DateModified = m.DateModified,
                             ChannelId = m.ChannelId,
                         };
            return result.Skip((page.Value - 1) * items).Take(items).AsAsyncEnumerable();
        }
    }
}
