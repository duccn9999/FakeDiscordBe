using DataAccesses.DTOs.BlockedUsers;

namespace DataAccesses.DTOs.PaginationModels.Users
{
    public class BlockedUsers
    {
        public List<GetBlockedUserDTO> Data {  get; set; }
        public int Pages { get; set; }
    }
}
