using DataAccesses.DTOs.GroupChats;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IGroupChatRepository : IGenericRepository<GroupChat>
    {
        public Task<IEnumerable<GetGroupChatDTO>> GetJoinedGroupChatsAsync(int userId);
        public Task<GetGroupChatDTO> GetGroupChatByIdAsync(int groupChatId);
    }
}
