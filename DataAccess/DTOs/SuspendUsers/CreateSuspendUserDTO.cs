using System.Text.Json.Serialization;

namespace DataAccesses.DTOs.SuspendUsers
{
    public class CreateSuspendUserDTO
    {
        public int UserId { get; set; }
        public int SuperAdminId { get; set; }
        public string? SuspendReason { get; set; }
        [JsonIgnore]
        public DateTime DateSuspend { get; set; } = DateTime.Now;
    }
}
