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
        }
    }
}
