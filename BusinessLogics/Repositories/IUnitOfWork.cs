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
        IAllowedRolesRepository AllowedRoles { get; }
        IAllowedUsersRepository AllowedUsers { get; }
        IUserFriendRepository UserFriends { get; }
        INotificationRepository Notifications { get; }
        IPrivateMessageImageRepository PrivateMessageImages { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
        int Save();
        Task<int> SaveAsync();
        void Dispose();
    }
}
