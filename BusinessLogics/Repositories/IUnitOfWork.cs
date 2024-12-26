using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IPrivateMessageRepository PrivateMsges { get; }
        IAuthenticationRepository Authentication { get; }
        IGroupChatRepository GroupChats { get; }
        IGroupChatParticipationRepository GroupChatParticipations { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
        int Save();
        void Dispose();
    }
}
