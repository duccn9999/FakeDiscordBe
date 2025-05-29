using DataAccesses.DTOs.UserRoles;

namespace DataAccesses.DTOs.PaginationModels.GroupChats
{
    public class NumberOfUserByEachRole
    {
        public List<GetNumberOfUserByEachRoleDTO> Data { get; set; }
        public int Pages { get; set; }
    }
}
