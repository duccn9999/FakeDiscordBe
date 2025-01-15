using DataAccesses.DTOs.GroupChats;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IGroupChatRepository : IGenericRepository<GroupChat>
    {
        public Task<IAsyncEnumerable<GetGroupChatDTO>> GetJoinedGroupChatsAsync(int userId);
        public Task<GetGroupChatDTO> GetGroupChatByIdAsync(int groupChatId);
        public Task<IAsyncEnumerable<GetGroupChatDTO>> GetJoinedGroupChatPaginationAsync(int userId, int? page, int items);
    }
}
