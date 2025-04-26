using BusinessLogics.Repositories;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.RepositoriesImpl
{
    public class PrivateMessageAttachmentRepository : GenericRepository<PrivateMessageAttachment>, IPrivateMessageAttachmentRepository
    {
        public PrivateMessageAttachmentRepository(FakeDiscordContext context) : base(context)
        {
        }
    }
}
