using BusinessLogics.Repositories;
using DataAccesses.DTOs.AllowedUsers;
using DataAccesses.Models;

namespace BusinessLogics.RepositoriesImpl
{
    public class AllowedUserRepository : GenericRepository<AllowedUser>, IAllowedUserRepository
    {
        public AllowedUserRepository(FakeDiscordContext context) : base(context)
        {
        }

        public IEnumerable<int> GetAllowedUsersByChannelId(int channelId)
        {
            var result = (
                from au in _context.AllowedUsers
                join u in _context.Users on au.UserId equals u.UserId
                where au.ChannelId == channelId
                select u.UserId
            );
            return result.AsEnumerable();
        }
    }
}
