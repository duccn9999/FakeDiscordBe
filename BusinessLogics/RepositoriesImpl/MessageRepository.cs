using BusinessLogics.Repositories;
using DataAccesses.DTOs.MessageAttachments;
using DataAccesses.DTOs.Messages;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogics.RepositoriesImpl
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(FakeDiscordContext context) : base(context) { }

        public async Task<IEnumerable<GetMessageDTO>> GetMessagesByChannelId(int channelId)
        {
            var result = from m in _context.Messages
                         join c in _context.Channels
                         on m.ChannelId equals c.ChannelId
                         join u in _context.Users
                         on m.UserCreated equals u.UserId
                         where c.ChannelId == channelId && u.IsActive
                         orderby m.DateCreated ascending
                         select new GetMessageDTO
                         {
                             MessageId = m.MessageId,
                             Username = u.UserName,
                             Avatar = u.Avatar,
                             Content = m.Content,
                             DateCreated = m.DateCreated.ToString("yyyy-MM-dd HH:mm"),
                             DateModified = m.DateModified,
                             ChannelId = m.ChannelId,
                             Attachments = m.Attachments.Select(i => new GetMessageAttachmentDTO
                             {
                                 AttachmentId = i.AttachmentId,
                                 Url = i.Url,
                                 MessageId = i.MessageId,
                                 ContentType = i.ContentType,
                                 DisplayName = i.DisplayName,
                                 PublicId = i.PublicId,
                                 DownloadLink = i.DownloadLink,
                             }).ToList(),
                         };
            return result.AsEnumerable();
        }

        public async Task<IEnumerable<GetMessageDTO>> GetMessagesPaginationByChannelId(int channelId, int page, int itemsPerPage)
        {
            var result = from m in _context.Messages
                         join c in _context.Channels
                         on m.ChannelId equals c.ChannelId
                         join u in _context.Users
                         on m.UserCreated equals u.UserId
                         where c.ChannelId == channelId && u.IsActive
                         orderby m.DateCreated descending
                         select new GetMessageDTO
                         {
                             MessageId = m.MessageId,
                             Username = u.UserName,
                             Avatar = u.Avatar,
                             Content = m.Content,
                             DateCreated = m.DateCreated.ToString("yyyy-MM-dd HH:mm"),
                             DateModified = m.DateModified,
                             ChannelId = m.ChannelId,
                             Attachments = m.Attachments.Select(i => new GetMessageAttachmentDTO
                             {
                                 AttachmentId = i.AttachmentId,
                                 Url = i.Url,
                                 MessageId = i.MessageId,
                                 ContentType = i.ContentType,
                                 DisplayName = i.DisplayName,
                                 PublicId = i.PublicId,
                                 DownloadLink = i.DownloadLink,
                             }).ToList(),
                         };
            return result
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .AsEnumerable()
                .Reverse(); // Reverse AFTER pagination
        }

        public async Task<IEnumerable<GetMessageDTO>> GetMessagesPaginationByPrivateChannelId(int channelId)
        {
            var result = from m in _context.Messages
                         join c in _context.Channels
                         on m.ChannelId equals c.ChannelId
                         join u in _context.Users
                         on m.UserCreated equals u.UserId
                         where c.ChannelId == channelId && u.IsActive
                         orderby m.DateCreated ascending
                         select new GetMessageDTO
                         {
                             MessageId = m.MessageId,
                             Username = u.UserName,
                             Avatar = u.Avatar,
                             Content = m.Content,
                             DateCreated = m.DateCreated.ToString("yyyy-MM-dd HH:mm"),
                             DateModified = m.DateModified,
                             ChannelId = m.ChannelId,
                             Attachments = m.Attachments.Select(i => new GetMessageAttachmentDTO
                             {
                                 AttachmentId = i.AttachmentId,
                                 Url = i.Url,
                                 MessageId = i.MessageId,
                                 ContentType = i.ContentType,
                                 DisplayName = i.DisplayName,
                                 PublicId = i.PublicId,
                                 DownloadLink = i.DownloadLink,
                             }).ToList(),
                         };
            return result.AsEnumerable();
        }

        public async Task<GetTagKeywordDTO> GetTagValue(int groupChatId, string keyword)
        {
            var query1 = from r in _context.Roles
                         where r.GroupChatId == groupChatId &&
                               r.RoleName.Equals(keyword, StringComparison.OrdinalIgnoreCase)
                         select new GetTagKeywordDTO
                         {
                             Id = r.RoleId,
                             Keyword = r.RoleName
                         };

            var query2 = from u in _context.Users
                         join ur in _context.UserRoles on u.UserId equals ur.UserId
                         join r in _context.Roles on ur.RoleId equals r.RoleId
                         where r.GroupChatId == groupChatId && u.IsActive &&
                               (r.RoleName.Equals(keyword, StringComparison.OrdinalIgnoreCase) ||
                                u.UserName.Equals(keyword, StringComparison.OrdinalIgnoreCase))
                         select new GetTagKeywordDTO
                         {
                             Id = u.UserId,
                             Keyword = u.UserName
                         };
            return await query1.Union(query2).FirstOrDefaultAsync();
        }
    }
}
