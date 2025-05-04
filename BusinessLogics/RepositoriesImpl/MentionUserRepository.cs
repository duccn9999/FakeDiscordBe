using BusinessLogics.Repositories;
using DataAccesses.DTOs.MentionUsers;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class MentionUserRepository: GenericRepository<MentionUser>, IMentionUserRepository
    {
        public MentionUserRepository(FakeDiscordContext context) : base(context)
        {
            
        }

        public List<GetMentionCountByUserDTO> GetMentionCountByUser(int userId, int channelId)
        {
            var result = (from c in _context.Channels
                          join m in _context.Messages on c.ChannelId equals m.ChannelId
                          join mu in _context.MentionUsers on m.MessageId equals mu.MessageId
                          where c.ChannelId == channelId && mu.UserId == userId && mu.IsRead == false
                          group mu by new { c.ChannelId, mu.UserId, mu.IsRead } into g
                          select new GetMentionCountByUserDTO
                          {
                              ChannelId = g.Key.ChannelId,
                              UserId = g.Key.UserId,
                              IsRead = g.Key.IsRead,
                              TotalMentions = g.Count()
                          }).ToList();
            return result;
        }
    }
}
