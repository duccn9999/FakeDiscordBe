using BusinessLogics.Repositories;
using DataAccesses.DTOs.Users;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogics.RepositoriesImpl
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(FakeDiscordContext context) : base(context) { }

        public async Task<bool> CheckAccoutExistedAsync(string userName, string password)
        {
            var user = await table.FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);
            return user != null;
        }

        public async Task<bool> CheckEmailDuplicatedAsync(string email)
        {
            var user = await table.FirstOrDefaultAsync(x => x.Email == email);
            return user != null;
        }

        public async Task<bool> CheckUserNameDuplicatedAsync(string userName)
        {
            var user = await table.FirstOrDefaultAsync(x => x.UserName == userName);
            return user != null;
        }

        public User GetByUserName(string userName)
        {
            var user = table.FirstOrDefault(x => x.UserName == userName);
            return user;
        }

        public UserParticipationDTO GetUserParticipations(string userName)
        {
            var userParticipations = (from u in _context.Users
                                      join p in _context.Participations
                                      on u.UserId equals p.UserId
                                      where u.UserName == userName
                                      group p by new
                                      {
                                          u.UserId,
                                          u.UserName,
                                          u.Avatar,
                                          u.CoverImage,
                                          u.Email
                                      } into groupedUsers
                                      select new UserParticipationDTO
                                      {
                                          UserId = groupedUsers.Key.UserId,
                                          UserName = groupedUsers.Key.UserName,
                                          Email = groupedUsers.Key.Email,
                                          Participations = groupedUsers
                                              .GroupBy(g => g.GroupChatId)
                                              .Select(g => new ParticipateInGroupsDTO
                                              {
                                                  GroupChatId = g.Key,
                                                  RoleId = g.Select(r => r.RoleId).Distinct().ToList()
                                              }).ToList()
                                      }).FirstOrDefault();
            var getByUserName = GetByUserName(userName);
            var result = userParticipations == null ? new UserParticipationDTO()
            {
                UserId = getByUserName.UserId,
                UserName = userName,
                Email = getByUserName.Email,
                Participations = null
            } 
            : userParticipations;
            return result;
        }

    }
}
