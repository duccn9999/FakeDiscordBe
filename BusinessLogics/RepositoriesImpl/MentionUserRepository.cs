using BusinessLogics.Repositories;
using DataAccesses.DTOs.MentionUsers;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogics.RepositoriesImpl
{
    public class MentionUserRepository: GenericRepository<MentionUser>, IMentionUserRepository
    {
        public MentionUserRepository(FakeDiscordContext context) : base(context)
        {
            
        }

        public async Task<GetMentionsCountDTO> GetMentionCountByUser(int userId, int channelId)
        {
            var count = await (from c in _context.Channels
                               join m in _context.Messages on c.ChannelId equals m.ChannelId
                               join mu in _context.MentionUsers on m.MessageId equals mu.MessageId
                               where c.ChannelId == channelId && mu.UserId == userId && mu.IsRead == false
                               select mu).CountAsync();

            return new GetMentionsCountDTO
            {
                ChannelId = channelId,
                MentionsCount = count
            };
        }



        public async Task MarkMentionsAsRead(int userId)
        {
             await _context.MentionUsers
                .Where(x => x.UserId == userId && !x.IsRead)
                .ExecuteUpdateAsync(s => s.SetProperty(m => m.IsRead, true));
        }
    }
}
