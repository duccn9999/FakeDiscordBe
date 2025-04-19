using AutoMapper;
using DataAccesses.DTOs.GroupChats;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class GroupChatProfile : Profile
    {
        public GroupChatProfile()
        {
            CreateMap<CreateGroupChatDTO, GroupChat>().ForMember(dest => dest.CoverImage, opt => opt.Ignore());
            CreateMap<UpdateGroupChatDTO, GroupChat>().ForMember(dest => dest.CoverImage, opt => opt.Ignore())
                .ForAllMembers(o => o.Condition((src, dest, value) => value != null));
            CreateMap<GroupChat, GetGroupChatDTO>();
        }

    }
}
