using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Security;

namespace BusinessLogics.RepositoriesImpl
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly FakeDiscordContext _context;
        private readonly IConfiguration _config;
        private IDbContextTransaction _transaction;
        public IUserRepository _userRepository;
        public IPrivateMessageRepository _privateMessageRepository;
        public IAuthenticationRepository _authenticationRepository;
        public IGroupChatRepository _groupChatRepository;
        public IChannelRepository _channelRepository;
        public IMessageRepository _messageRepository;
        public IRoleRepository _roleRepository;
        public IUserRoleRepository _userRoles;
        public IPermissionRepository _permissionRepository;
        public IRolePermissionRepository _rolePermission;
        public IAllowedUserRepository _allowedUsersRepository;
        public IAllowedRoleRepository _allowedRolesRepository;
        public IUserFriendRepository _userFriendRepository;
        public INotificationRepository _notificationRepository;
        public IPrivateMessageAttachmentRepository _privateMessageAttachmentRepository;
        public IMessageAttachmentRepository _messageAttachmentRepository;
        public IMentionUserRepository _mentionUserRepository;
        public ILastSeenMessageRepository _lastSeenMessageRepository;
        public UnitOfWork(FakeDiscordContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public IPrivateMessageRepository PrivateMsges => _privateMessageRepository ??= new PrivateMessageRepository(_context);
        public IUserRoleRepository UserRoles => _userRoles ??= new UserRoleRepository(_context);
        public IAuthenticationRepository Authentication => _authenticationRepository ??= new AuthenticationRepository(Users, _config, UserRoles);
        public IGroupChatRepository GroupChats => _groupChatRepository ??= new GroupChatRepository(_context);
        public IChannelRepository Channels => _channelRepository ??= new ChannelRepository(_context);
        public IMessageRepository Messages => _messageRepository ??= new MessageRepository(_context);
        public IRoleRepository Roles => _roleRepository ??= new RoleRepository(_context);
        public IPermissionRepository Permissions => _permissionRepository ??= new PermissionRepository(_context);

        public IRolePermissionRepository RolePermissions => _rolePermission ??= new RolePermissionRepository(_context);
        public IAllowedRoleRepository AllowedRoles => _allowedRolesRepository ??= new AllowedRoleRepository(_context);
        public IAllowedUserRepository AllowedUsers => _allowedUsersRepository ??= new AllowedUserRepository(_context);
        public IUserFriendRepository UserFriends => _userFriendRepository ??= new UserFriendRepository(_context);
        public INotificationRepository Notifications => _notificationRepository ??= new NotificationRepository(_context);
        public IPrivateMessageAttachmentRepository PrivateMessageAttachments => _privateMessageAttachmentRepository ??= new PrivateMessageAttachmentRepository(_context);
        public IMessageAttachmentRepository MessageAttachments => _messageAttachmentRepository ??= new MessageAttachmentRepository(_context);

        public IMentionUserRepository MentionUsers => _mentionUserRepository ??= new MentionUserRepository(_context);
        public ILastSeenMessageRepository LastSeenMessages => _lastSeenMessageRepository ??= new LastSeenMessageRepository(_context);
        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
