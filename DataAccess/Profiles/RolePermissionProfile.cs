using AutoMapper;
using DataAccesses.DTOs.RolePermissions;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class RolePermissionProfile : Profile
    {
        public RolePermissionProfile()
        {
            CreateMap<RolePermissionDTO, RolePermission>();
        }
    }
}
