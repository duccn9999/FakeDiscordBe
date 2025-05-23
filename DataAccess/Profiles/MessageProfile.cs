using AutoMapper;
using DataAccesses.DTOs.Messages;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<CreateMessageDTO, Message>()
                .ForMember(dest => dest.Attachments, opt => opt.Ignore())
                .ForMember(dest => dest.MentionUsers, opt => opt.Ignore());
            CreateMap<UpdateMessageDTO, Message>()
                .ForMember(dest => dest.Attachments, opt => opt.Ignore())
                .ForMember(dest => dest.MentionUsers, opt => opt.Ignore()); ;
            CreateMap<Message, GetMessageDTO>();
        }
    }
}
