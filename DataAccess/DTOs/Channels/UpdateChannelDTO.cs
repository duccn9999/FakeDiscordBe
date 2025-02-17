using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccesses.DTOs.Channels
{
    public class UpdateChannelDTO
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        [JsonIgnore]
        public DateTime? DateModified { get; set; } = DateTime.Now;
        public int? UserModified { get; set; }
    }
}
