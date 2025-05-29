using AutoMapper;
using DataAccesses.DTOs.BlockedUsers;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class BlockedFriendProfile : Profile
    {
        public BlockedFriendProfile()
        {
            CreateMap<CreateBlockedUserDTO, BlockedUser>();   
        }
    }
}
