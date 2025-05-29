using BusinessLogics.Repositories;
using DataAccesses.DTOs.PaginationModels.GroupChats;
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
                        join g in _context.GroupChats
                        on r.GroupChatId equals g.GroupChatId
                        where ur.UserId == userId && g.IsActive
                        select new GetAllRolesByUserDTO
                        {
                            GroupChatId = r.GroupChatId,
                            RoleId = r.RoleId,
                            RoleName = r.RoleName
                        };
            return query.ToList();
        }

        public NumberOfUserByEachRole GetNumberOfUserByEachRole(int groupChatId, int page, int itemsPerPage, string? keyword)
        {
            var query = _context.Roles
                .Where(r => r.GroupChatId == groupChatId
                            && r.RoleName != RolesSeed.ADMINISTRATOR_ROLE
                            && r.RoleName != RolesSeed.MEMBER_ROLE)
                .Where(r => _context.GroupChats.Any(g => g.GroupChatId == r.GroupChatId && g.IsActive))
                .Where(r => string.IsNullOrEmpty(keyword) || r.RoleName.Contains(keyword))
                .Select(r => new GetNumberOfUserByEachRoleDTO
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    GroupChatId = r.GroupChatId,
                    Total = _context.UserRoles
                        .Where(ur => ur.RoleId == r.RoleId)
                        .Join(_context.Users, ur => ur.UserId, u => u.UserId, (ur, u) => u)
                        .Count(u => u.IsActive),
                    Color = r.Color,
                    DateCreated = r.DateCreated.ToString("dd/MM/yyyy HH:mm"),
                    DateModified = r.DateModified.HasValue ? r.DateModified.Value.ToString("dd/MM/yyyy HH:mm") : null,
                });

            int totalCount = query.Count();
            int pages = (int)Math.Ceiling((double)totalCount / itemsPerPage);

            var pagedData = query
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            return new NumberOfUserByEachRole
            {
                Data = pagedData,
                Pages = pages
            };
        }

        public async Task<GetNumberOfUserByEachRoleDTO> GetNumberOfUserByRole(int groupChatId, int roleId)
        {
            var query = from r in _context.Roles
                        join g in _context.GroupChats on r.GroupChatId equals g.GroupChatId
                        where g.IsActive == true
                        && r.GroupChatId == groupChatId
                        && r.RoleId == roleId
                        join ur in _context.UserRoles on r.RoleId equals ur.RoleId into urGroup
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
                        join g in _context.GroupChats
                        on r.GroupChatId equals g.GroupChatId
                        where ur.UserId == userId && r.GroupChatId == groupChatId && g.IsActive
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
                        join g in _context.GroupChats
                        on r.GroupChatId equals g.GroupChatId
                        where r.RoleId == roleId && r.GroupChatId == groupChatId && u.IsActive && g.IsActive
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
                        where ur.RoleId == roleId && u.IsActive
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
                         where r.GroupChatId == groupChatId && (ur != null && ur.RoleId != roleId) && u.IsActive
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
