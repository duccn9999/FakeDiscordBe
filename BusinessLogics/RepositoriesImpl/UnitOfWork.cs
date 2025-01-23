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
        public IPrivateMessageRepository _messageRepository;
        public IAuthenticationRepository _authenticationRepository;
        public IGroupChatRepository _groupChatRepository;
        public IParticipationRepository _participationRepository;
        public IChannelRepository _channelRepository;
        public UnitOfWork(FakeDiscordContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public IPrivateMessageRepository PrivateMsges => _messageRepository ??= new PrivateMessageRepository(_context);
        public IAuthenticationRepository Authentication => _authenticationRepository ??= new AuthenticationRepository(Users, _config);
        public IGroupChatRepository GroupChats => _groupChatRepository ??= new GroupChatRepository(_context);
        public IParticipationRepository Participations => _participationRepository ??= new ParticipationRepository(_context);
        public IChannelRepository Channels => _channelRepository ??= new ChannelRepository(_context);
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
