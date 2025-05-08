using DataAccesses.DTOs.MentionUsers;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IMentionUserRepository : IGenericRepository<MentionUser>
    {
        public Task<GetMentionsCountDTO> GetMentionCountByUser(int userId, int channelId);
        public Task MarkMentionsAsRead(int userId);
    }
}
