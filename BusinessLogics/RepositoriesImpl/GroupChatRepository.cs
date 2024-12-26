using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.RepositoriesImpl
{
    public class GroupChatRepository : GenericRepository<GroupChat>, IGroupChatRepository
    {
        public GroupChatRepository(FakeDiscordContext context) : base(context) 
        {
        }

        public async Task<IEnumerable<GroupChat>> GetJoinedGroupChatsAsync(int userId)
        {
            var joinedGroupChat = await (from g in _context.GroupChats
                                  join gp in _context.Participations
                                  on  g.GroupChatId equals gp.GroupChatId
                                  where gp.UserId == userId
                                  select g).ToListAsync();
            return joinedGroupChat;
        }
    }
}
