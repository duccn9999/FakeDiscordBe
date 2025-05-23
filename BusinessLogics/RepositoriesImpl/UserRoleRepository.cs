using BusinessLogics.Repositories;
using DataAccesses.DTOs.UserRoles;
using DataAccesses.DTOs.Users;
using DataAccesses.Models;
using DataAccesses.Utils;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogics.RepositoriesImpl
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(FakeDiscordContext context) : base(context)
        {

        }

        public List<GetAllRolesByUserDTO> GetAllRolesByUser(int userId)
        {
            var query = from r in _context.Roles
                        join ur in _context.UserRoles
                        on r.RoleId equals ur.RoleId
                        where ur.UserId == userId
                        select new GetAllRolesByUserDTO
                        {
                            GroupChatId = r.GroupChatId,
                            RoleId = r.RoleId,
                            RoleName = r.RoleName
                        };
            return query.ToList();
        }

        public List<GetNumberOfUserByEachRoleDTO> GetNumberOfUserByEachRole(int groupChatId)
        {
            var query = from r in _context.Roles
                        join ur in _context.UserRoles
                        on r.RoleId equals ur.RoleId
                        into urGroup
                        from ur in urGroup.DefaultIfEmpty()
                        join u in _context.Users
                        on ur.UserId equals u.UserId
                        into uGroup
                        from u in uGroup.DefaultIfEmpty()
                        group u by new { r.RoleId, r.RoleName, r.GroupChatId } into grouped
                        where grouped.Key.GroupChatId == groupChatId && (grouped.Key.RoleName != RolesSeed.ADMINISTRATOR_ROLE && grouped.Key.RoleName != RolesSeed.MEMBER_ROLE)
                        select new GetNumberOfUserByEachRoleDTO
                        {
                            RoleId = grouped.Key.RoleId,
                            RoleName = grouped.Key.RoleName,
                            GroupChatId = grouped.Key.GroupChatId,
                            Total = grouped.Count(u => u != null)
                        };
            return query.ToList();
        }

        public async Task<GetNumberOfUserByEachRoleDTO> GetNumberOfUserByRole(int groupChatId, int roleId)
        {
            var query = from r in _context.Roles
                        join ur in _context.UserRoles on r.RoleId equals ur.RoleId into urGroup
                        where r.GroupChatId == groupChatId && r.RoleId == roleId
                        select new GetNumberOfUserByEachRoleDTO
                        {
                            RoleId = r.RoleId,
                            RoleName = r.RoleName,
                            GroupChatId = r.GroupChatId,
                            Total = urGroup.Count()
                        };

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<UserRoleDTO>> GetRolesByUserInGroupChat(int groupChatId, int userId)
        {
            var query = from ur in _context.UserRoles
                        join r in _context.Roles
                        on ur.RoleId equals r.RoleId
                        where ur.UserId == userId && r.GroupChatId == groupChatId
                        select new UserRoleDTO
                        {
                            UserId = ur.UserId,
                            RoleId = r.RoleId,
                        };
            var result = await query.ToListAsync();
            return result;
        }

        public List<UserRole> GetUserRolesByUserId(int userId)
        {
            var result = _context.UserRoles.Where(x => x.UserId == userId).ToList();
            return result;
        }

        public async Task<List<GetUsersByEachRoleDTO>> GetUsersByEachRole(int groupChatId, int roleId)
        {
            var query = from r in _context.Roles
                        join ur in _context.UserRoles
                        on r.RoleId equals ur.RoleId
                        join u in _context.Users
                        on ur.UserId equals u.UserId
                        where r.RoleId == roleId && r.GroupChatId == groupChatId
                        select new GetUsersByEachRoleDTO
                        {
                            UserId = u.UserId,
                            UserName = u.UserName,
                            GroupChatId = r.GroupChatId,
                            RoleId = r.RoleId
                        };
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<List<GetUserDTO>> GetUsersByRole(int roleId)
        {
            var query = from ur in _context.UserRoles
                        join u in _context.Users on ur.UserId equals u.UserId
                        where ur.RoleId == roleId
                        select new GetUserDTO
                        {
                            UserId = u.UserId,
                            UserName = u.UserName,
                            Avatar = u.Avatar
                        };
            return query.ToList();
        }

        public async Task<List<GetUsersNotInRoleDTO>> GetUsersNotInRole(int groupChatId, int roleId)
        {
            var query = (from r in _context.Roles
                         join ur in _context.UserRoles on r.RoleId equals ur.RoleId into urGroup
                         from ur in urGroup.DefaultIfEmpty()
                         join u in _context.Users on ur.UserId equals u.UserId into uGroup
                         from u in uGroup.DefaultIfEmpty()
                         where r.GroupChatId == groupChatId && (ur != null && ur.RoleId != roleId)
                         select new GetUsersNotInRoleDTO
                         {
                             UserId = u.UserId,
                             CoverImage = u.Avatar,
                             UserName = u.UserName
                         }).Distinct();
            return query.ToList();
        }
    }
}
