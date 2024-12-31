using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.Channels
{
    public class GetChannelsDTO
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
    }
}
