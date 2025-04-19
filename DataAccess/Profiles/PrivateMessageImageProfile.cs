using AutoMapper;
using DataAccesses.DTOs.PrivateMessageImages;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class PrivateMessageImageProfile : Profile
    {
        public PrivateMessageImageProfile()
        {
            CreateMap<CreatePrivateMessageImageDTO, PrivateMessageImage>();
        }
    }
}
