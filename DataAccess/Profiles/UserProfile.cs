using AutoMapper;
using DataAccesses.DTOs.Users;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDTO, User>();
            CreateMap<ChangeEmailDTO, User>();
            CreateMap<ChangeUsernameDTO, User>();
            CreateMap<ChangePasswordDTO, User>();
            CreateMap<ChangeAvatarDTO, User>();
            CreateMap<User, LoginUserDTO>();
        }
    }
}
