using AutoMapper;
using DataAccesses.DTOs.SuspendGroupChats;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class SuspendGroupChatProfile : Profile
    {
        public SuspendGroupChatProfile()
        {
            CreateMap<CreateSuspendGroupChatDTO, SuspendGroupChat>();
        }
    }
}
