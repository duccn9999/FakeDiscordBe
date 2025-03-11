using AutoMapper;
using DataAccesses.DTOs.GroupChats;
using DataAccesses.DTOs.Roles;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<CreateRoleDTO, Role>();
            CreateMap<UpdateRoleDTO, Role>();
        }
    }
}
