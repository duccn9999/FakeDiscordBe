using AutoMapper;
using DataAccesses.DTOs.Channels;
using DataAccesses.DTOs.GroupChats;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Profiles
{
    public class ChannelProfile : Profile
    {
        public ChannelProfile()
        {
            CreateMap<CreateChannelDTO, Channel>();
            CreateMap<UpdateChannelDTO, Channel>();
            CreateMap<CreatePrivateChannelDTO, Channel>()
                .ForMember(dest => dest.AllowedUsers, opt => opt.Ignore())
                .ForMember(dest => dest.AllowedRoles, opt => opt.Ignore());
        }
    }
}
