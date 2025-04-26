using AutoMapper;
using DataAccesses.DTOs.PrivateMessageAttachments;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class PrivateMessageImageProfile : Profile
    {
        public PrivateMessageImageProfile()
        {
            CreateMap<CreatePrivateMessageAttachmentDTO, PrivateMessageAttachment>();
        }
    }
}
