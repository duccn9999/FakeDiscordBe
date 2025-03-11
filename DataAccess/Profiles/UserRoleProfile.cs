using AutoMapper;
using DataAccesses.DTOs.GroupChatRoles;
using DataAccesses.DTOs.UserRoles;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Profiles
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRoleDTO, UserRole>();
        }
    }
}
