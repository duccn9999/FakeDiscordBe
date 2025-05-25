using AutoMapper;
using DataAccesses.DTOs.SuspendUsers;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class SuspendUserProfile : Profile
    {
        public SuspendUserProfile()
        {
            CreateMap<CreateSuspendUserDTO, SuspendUser>();
        }
    }
}
