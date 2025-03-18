using AutoMapper;
using DataAccesses.DTOs.UserGroupChats;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Profiles
{
    public class UserGroupChatProfile : Profile
    {
        public UserGroupChatProfile()
        {
            CreateMap<UserGroupChatDTO, UserGroupChat>();
        }
    }
}
