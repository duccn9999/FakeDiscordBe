namespace DataAccesses.DTOs.Channels
{
    public class UpdatePrivateChannelPermissionsDTO
    {
        public List<int>? AllowedRoles { get; set; }
        public List<int>? AllowedUsers { get; set; }
    }
}
