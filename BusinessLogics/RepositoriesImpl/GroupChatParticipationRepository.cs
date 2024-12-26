using BusinessLogics.Repositories;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.RepositoriesImpl
{
    public class GroupChatParticipationRepository : GenericRepository<GroupChatParticipation>, IGroupChatParticipationRepository
    {
        public GroupChatParticipationRepository(FakeDiscordContext context) : base(context)
        {
        }
    }
}
