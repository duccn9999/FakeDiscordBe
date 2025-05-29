using DataAccesses.DTOs.Roles;

namespace DataAccesses.DTOs.PaginationModels.Roles
{
    public class Roles
    {
        public List<GetRoleDTO> roleDTO {  get; set; }
        public int Pages { get; set; }
    }
}
