using BusinessLogics.Repositories;
using DataAccesses.DTOs.UserRoles;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.RepositoriesImpl
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(FakeDiscordContext context) : base(context)
        {

        }

        public List<GetNumberOfUserByEachRoleDTO> GetNumberOfUserByEachRole(int groupChatId)
        {
            var result = _context.Roles
                .GroupJoin(
                    _context.UserRoles
                        .Join(_context.GroupChatRoles,
                            userRole => userRole.RoleId,
                            groupChatRole => groupChatRole.RoleId,
                            (userRole, groupChatRole) => new { userRole.RoleId, groupChatRole.GroupChatId, userRole.UserId }
                        )
                        .Where(x => x.GroupChatId == groupChatId), // Ensure filtering happens before join
                    role => role.RoleId,
                    userRoleGroup => userRoleGroup.RoleId,
                    (role, userRoleGroup) => new GetNumberOfUserByEachRoleDTO
                    {
                        RoleId = role.RoleId,
                        RoleName = role.RoleName,
                        GroupChatId = groupChatId, // Since all records are filtered by this, it can be assigned directly
                        Total = userRoleGroup.Count()
                    }
                )
                .ToList();

            return result;
        }

        public async Task<GetNumberOfUserByEachRoleDTO> GetNumberOfUserByRole(int groupChatId, int roleId)
        {
            var query = from r in _context.Roles
                        join gcr in _context.GroupChatRoles on r.RoleId equals gcr.RoleId
                        join g in _context.GroupChats on gcr.GroupChatId equals g.GroupChatId
                        join ur in _context.UserRoles on r.RoleId equals ur.RoleId into urGroup
                        where g.GroupChatId == groupChatId && r.RoleId == roleId
                        select new GetNumberOfUserByEachRoleDTO
                        {
                            RoleId = r.RoleId,
                            RoleName = r.RoleName,
                            GroupChatId = g.GroupChatId,
                            Total = urGroup.Count()
                        };

            var result = await query.FirstOrDefaultAsync();
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
                        join ur in _context.UserRoles on r.RoleId equals ur.RoleId into urGroup
                        from ur in urGroup.DefaultIfEmpty() // LEFT JOIN on UserRole
                        join gcr in _context.GroupChatRoles on r.RoleId equals gcr.RoleId
                        join u in _context.Users on ur.UserId equals u.UserId
                        where r.RoleId == roleId && gcr.GroupChatId == groupChatId
                        select new GetUsersByEachRoleDTO
                        {
                            UserId = u.UserId,
                            UserName = u.UserName,
                            GroupChatId = gcr.GroupChatId,
                            RoleId = r.RoleId
                        };
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<List<GetUserNotInRoleDTO>> GetUsersNotInRole(int groupChatId, int roleId)
        {
            var query = from ur in _context.UserRoles
                        join u in _context.Users on ur.UserId equals u.UserId
                        join ugc in _context.UserGroupChats on u.UserId equals ugc.UserId
                        where ugc.GroupChatId == groupChatId && ur.RoleId != roleId
                        select new GetUserNotInRoleDTO
                        {
                            UserId = u.UserId,
                            CoverImage = u.CoverImage,
                            UserName = u.UserName
                        };
            var result = await query.Distinct().ToListAsync();
            return result;
        }
    }
}
