using BusinessLogics.Repositories;
using DataAccesses.DTOs.UserGroupChats;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.RepositoriesImpl
{
    public class UserGroupChatRepository : GenericRepository<UserGroupChat>, IUserGroupChatRepository
    {
        public UserGroupChatRepository(FakeDiscordContext context) : base(context)
        {
            
        }
    }
}
