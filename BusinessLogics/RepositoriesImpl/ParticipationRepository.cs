using BusinessLogics.Repositories;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.RepositoriesImpl
{
    public class ParticipationRepository : GenericRepository<Participation>, IParticipationRepository
    {
        public ParticipationRepository(FakeDiscordContext context) : base(context)
        {
        }
    }
}
