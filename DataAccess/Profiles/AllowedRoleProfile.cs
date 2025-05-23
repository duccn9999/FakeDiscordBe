using AutoMapper;
using DataAccesses.DTOs.AllowedRoles;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Profiles
{
    public class AllowedRoleProfile : Profile
    {
        public AllowedRoleProfile()
        {
            CreateMap<CreateAllowedRoleDTO, AllowedRole>();
        }
    }
}
