using DataAccesses.DTOs.GroupChats;
using DataAccesses.DTOs.UserGroupChats;
using DataAccesses.Models;

namespace BusinessLogics.Repositories
{
    public interface IGroupChatRepository : IGenericRepository<GroupChat>
    {
        public Task<IEnumerable<GetGroupChatDTO>> GetJoinedGroupChatsAsync(int userId);
        public Task<GetGroupChatDTO> GetGroupChatByIdAsync(int groupChatId);
        public Task<GroupChat> GetGroupChatByChannelIdAsync(int channelId);
        public Task<IEnumerable<GetGroupChatDTO>> GetJoinedGroupChatPaginationAsync(int userId, int? page, int items);
        public Task<GroupChat> GetGroupChatByInviteCode(string inviteCode);
    }
}
