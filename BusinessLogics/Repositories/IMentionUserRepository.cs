using DataAccesses.DTOs.MentionUsers;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IMentionUserRepository : IGenericRepository<MentionUser>
    {
        public List<GetMentionCountByUserDTO> GetMentionCountByUser(int userId, int channelId);
    }
}
