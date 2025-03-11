using AutoMapper;
using DataAccesses.DTOs.GroupChatRoles;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class GroupChatRoleProfile : Profile
    {
        public GroupChatRoleProfile()
        {
            CreateMap<GroupChatRoleDTO, GroupChatRole>();
        }
    }
}
