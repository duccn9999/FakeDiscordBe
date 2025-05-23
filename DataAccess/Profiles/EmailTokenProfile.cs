using AutoMapper;
using DataAccesses.DTOs.EmailTokens;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class EmailTokenProfile : Profile
    {
        public EmailTokenProfile()
        {
            CreateMap<CreateEmailTokenDTO, EmailToken>();
        }
    }
}
