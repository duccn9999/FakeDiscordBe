﻿using System;
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
        IGroupChatRoleRepository GroupChatRoles { get; }
        IUserRoleRepository UserRoles { get; }
        IPermissionRepository Permissions { get; }
        IRolePermissionRepository RolePermissions { get; }
        IUserGroupChatRepository UserGroupChats { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
        int Save();
        Task<int> SaveAsync();
        void Dispose();
    }
}
