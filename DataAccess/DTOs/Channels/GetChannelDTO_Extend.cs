﻿namespace DataAccesses.DTOs.Channels
{
    public class GetChannelDTO_Extend : GetChannelDTO
    {
        public int GroupChatId { get; set; }
        public bool IsPrivate { get; set; }
    }
}
