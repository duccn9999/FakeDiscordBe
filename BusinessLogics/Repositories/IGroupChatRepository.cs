using DataAccesses.DTOs.GroupChats;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IGroupChatRepository : IGenericRepository<GroupChat>
    {
        public Task<IEnumerable<GroupChat>> GetJoinedGroupChatsAsync(int userId);
    }
}
