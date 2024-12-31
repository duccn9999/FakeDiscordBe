﻿using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.GroupChats;
using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.RepositoriesImpl
{
    public class GroupChatRepository : GenericRepository<GroupChat>, IGroupChatRepository
    {
        public GroupChatRepository(FakeDiscordContext context) : base(context) 
        {
        }

        public async Task<IEnumerable<GetJoinedGroupChatsDTO>> GetJoinedGroupChatsAsync(int userId)
        {
            var result = from g in _context.GroupChats
                         join p in _context.Participations
                         on g.GroupChatId equals p.GroupChatId
                         join u in _context.Users
                         on p.UserId equals u.UserId
                         where u.UserId == userId
                         select new GetJoinedGroupChatsDTO
                         {
                             GroupChatId = g.GroupChatId,
                             Name = g.Name,
                             CoverImage = g.CoverImage
                         };
            return result.AsEnumerable();
        }
    }
}
