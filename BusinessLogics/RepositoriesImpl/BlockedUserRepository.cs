using BusinessLogics.Repositories;
using DataAccesses.DTOs.BlockedUsers;
using DataAccesses.DTOs.PaginationModels.Users;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.RepositoriesImpl
{
    public class BlockedUserRepository : GenericRepository<BlockedUser>, IBlockUserRepository
    {
        public BlockedUserRepository(FakeDiscordContext context) : base(context)
        {
            
        }

        public BlockedUsers GetBlockedUsersPagination(int userId, int page, int itemsPerPage, string? keyword)
        {
            var query = from bu in _context.BlockedUsers
                        join u in _context.Users
                        on bu.UserId2 equals u.UserId // Changed from UserId1 to UserId2 to get blocked user info
                        where bu.UserId1 == userId    // Filter by the user who blocked others
                        select new GetBlockedUserDTO
                        {
                            Id = bu.Id,
                            UserId1 = bu.UserId1,
                            UserId2 = bu.UserId2,
                            Avatar = u.Avatar,
                            Username = u.UserName,    // Fixed: was u.Avatar, should be u.Username
                            BlockedDate = bu.BlockedDate.ToString("dd/MM/yyyy HH:mm")
                        };

            // Apply keyword filter if provided
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.Username.Contains(keyword));
            }

            int totalCount = query.Count();
            int totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling((double)totalCount / itemsPerPage);

            var paginatedData = query
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            return new BlockedUsers
            {
                Data = paginatedData,
                Pages = totalPages
            };
        }
    }
}
