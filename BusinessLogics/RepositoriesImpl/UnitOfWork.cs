using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

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
        public IGroupChatRoleRepository _groupChatRoles;

        public IUserRoleRepository _userRoles;
        public UnitOfWork(FakeDiscordContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public IPrivateMessageRepository PrivateMsges => _privateMessageRepository ??= new PrivateMessageRepository(_context);
        public IAuthenticationRepository Authentication => _authenticationRepository ??= new AuthenticationRepository(Users, _config);
        public IGroupChatRepository GroupChats => _groupChatRepository ??= new GroupChatRepository(_context);
        public IChannelRepository Channels => _channelRepository ??= new ChannelRepository(_context);
        public IMessageRepository Messages => _messageRepository ??= new MessageRepository(_context);
        public IRoleRepository Roles => _roleRepository ??= new RoleRepository(_context);
        public IUserRoleRepository UserRoles => _userRoles ??= new UserRoleRepository(_context);
        public IGroupChatRoleRepository GroupChatRoles => _groupChatRoles ?? new GroupChatRoleRepository(_context);

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
