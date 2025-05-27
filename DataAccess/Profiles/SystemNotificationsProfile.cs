using AutoMapper;
using DataAccesses.DTOs.SystemNotifications;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class SystemNotificationsProfile : Profile
    {
        public SystemNotificationsProfile()
        {
            CreateMap<CreateSystemNotificationModel, SystemNotification>();
        }
    }
}
