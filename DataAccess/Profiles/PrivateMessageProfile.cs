﻿using AutoMapper;
using DataAccesses.DTOs.PrivateMessages;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class PrivateMessageProfile : Profile
    {
        public PrivateMessageProfile()
        {
            CreateMap<CreatePrivateMessageDTO, PrivateMessage>().ForMember(dest => dest.Attachments, opt => opt.Ignore());
            CreateMap<UpdatePrivateMessageDTO, PrivateMessage>();
        }
    }
}
