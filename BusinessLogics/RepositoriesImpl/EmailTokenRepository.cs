using BusinessLogics.Repositories;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class EmailTokenRepository : GenericRepository<EmailToken>,IEmailTokenRepository
    {
        public EmailTokenRepository(FakeDiscordContext context) : base(context)
        {
            
        }
    }
}
