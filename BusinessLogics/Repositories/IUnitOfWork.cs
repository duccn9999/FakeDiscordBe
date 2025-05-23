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
        IChannelRepository Channels { get; }
        IMessageRepository Messages { get; }
        IRoleRepository Roles { get; }
        IUserRoleRepository UserRoles { get; }
        IPermissionRepository Permissions { get; }
        IRolePermissionRepository RolePermissions { get; }
        IAllowedRoleRepository AllowedRoles { get; }
        IAllowedUserRepository AllowedUsers { get; }
        IUserFriendRepository UserFriends { get; }
        INotificationRepository Notifications { get; }
        IPrivateMessageAttachmentRepository PrivateMessageAttachments { get; }
        IMessageAttachmentRepository MessageAttachments { get; }
        IMentionUserRepository MentionUsers { get; }
        ILastSeenMessageRepository LastSeenMessages { get; }
        IEmailRepository Emails { get; }
        IEmailTokenRepository EmailTokens { get; }
        IGroupChatBlackListRepository GroupChatBlackLists { get; }
        ISuperAdminRepository SuperAdmins { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
        int Save();
        Task<int> SaveAsync();
        void Dispose();
    }
}
