using BusinessLogics.Repositories;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class MessageAttachmentRepository : GenericRepository<MessageAttachment> ,IMessageAttachmentRepository
    {
        public MessageAttachmentRepository(FakeDiscordContext context) : base(context)
        {
            
        }
    }
}
