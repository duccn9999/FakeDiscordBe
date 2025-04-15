using AutoMapper;
using DataAccesses.DTOs.UserFriends;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class UserFriendProfile : Profile
    {
        public UserFriendProfile()
        {
            CreateMap<SendFriendRequestDTO, UserFriend>();
            CreateMap<UserFriend, GetUserFriendDTO>();
        }
    }
}
