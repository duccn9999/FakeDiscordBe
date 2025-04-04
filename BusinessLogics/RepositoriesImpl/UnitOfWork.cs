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
        public IAllowedUsersRepository _allowedUsersRepository;
        public IAllowedRolesRepository _allowedRolesRepository;
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
        public IAllowedRolesRepository AllowedRoles => _allowedRolesRepository ??= new AllowedRolesRepository(_context);
        public IAllowedUsersRepository AllowedUsers => _allowedUsersRepository ??= new AllowedUsersRepository(_context);
        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            Save();
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
