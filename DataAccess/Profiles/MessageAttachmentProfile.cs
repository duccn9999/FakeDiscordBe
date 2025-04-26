using AutoMapper;
using DataAccesses.DTOs.MessageAttachments;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Profiles
{
    internal class MessageAttachmentProfile : Profile
    {
        public MessageAttachmentProfile()
        {
            CreateMap<CreateMessageAttachmentDTO, MessageAttachment>();
        }
    }
}
