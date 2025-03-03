using AutoMapper;
using DataAccesses.DTOs.GroupChats;
using DataAccesses.DTOs.Users;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Profiles
{
    public class GroupChatProfile : Profile
    {
        public GroupChatProfile()
        {
            CreateMap<CreateGroupChatDTO, GroupChat>();
            CreateMap<UpdateGroupChatDTO, GroupChat>();
        }

    }
}
