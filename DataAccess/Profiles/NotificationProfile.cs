using AutoMapper;
using DataAccesses.DTOs.Notifications;
using DataAccesses.Models;

namespace DataAccesses.Profiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<CreateNotificationDTO, Notification>();
        }
    }
}
