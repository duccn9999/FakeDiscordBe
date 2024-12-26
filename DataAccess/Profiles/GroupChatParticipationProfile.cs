using AutoMapper;
using DataAccesses.DTOs.GroupChatParticipations;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Profiles
{
    public class GroupChatParticipationProfile : Profile
    {
        public GroupChatParticipationProfile()
        {
            CreateMap<AddGroupChatParticipation, GroupChatParticipation>();
        }
    }
}
