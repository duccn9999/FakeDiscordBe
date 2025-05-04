using AutoMapper;
using DataAccesses.DTOs.LastSeenMessages;
using DataAccesses.Models;

public class LastSeenMessageProfile : Profile
{
    public LastSeenMessageProfile()
    {
        CreateMap<CreateLastSeenMessageDTO, LastSeenMessage>();
    }
}
