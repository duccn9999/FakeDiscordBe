using AutoMapper;
using DataAccesses.DTOs.GroupChatBlackLists;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class GroupChatBlackListProfile : Profile
    {
        public GroupChatBlackListProfile()
        {
            CreateMap<AddToBlackListDTO, GroupChatBlackList>();
        }
    }
}
