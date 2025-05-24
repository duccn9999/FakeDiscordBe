using System.Text.Json.Serialization;

namespace DataAccesses.DTOs.SuspendGroupChats
{
    public class CreateSuspendGroupChatDTO
    {
        public int GroupChatId { get; set; }
        public int SuperAdminId { get; set; }
        public string? SuspendReason { get; set; }
        [JsonIgnore]
        public DateTime DateSuspend { get; set; } = DateTime.Now;
    }
}
