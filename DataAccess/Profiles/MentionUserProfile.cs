using AutoMapper;
using DataAccesses.DTOs.MentionUsers;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class MentionUserProfile : Profile
    {
        public MentionUserProfile()
        {
            CreateMap<CreateMentionUserDTO, MentionUser>();
        }
    }
}
