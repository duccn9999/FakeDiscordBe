using AutoMapper;
using DataAccesses.DTOs.Permissions;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Profiles
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<PermissionDTO, Permission>();
            CreateMap<Permission, PermissionDTO>();
        }
    }
}
