using AutoMapper;
using DataAccesses.DTOs.AllowedUsers;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class AllowedUserProfile : Profile
    {
        public AllowedUserProfile()
        {
            CreateMap<CreateAllowedUserDTO, AllowedUser>();
        }
    }
}
