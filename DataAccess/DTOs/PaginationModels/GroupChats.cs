using DataAccesses.DTOs.GroupChats;
using System.ComponentModel.DataAnnotations;

namespace DataAccesses.DTOs.PaginationModels
{
    public class GroupChatPaginationDTO
    {
        public int GroupChatId { get; set; }
        public string Name { get; set; }
        public string? CoverImage { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserCreated { get; set; }
        public bool IsActive { get; set; }
    }
    public class GroupChats
    {
        public List<GroupChatPaginationDTO> Data;
        public int Pages;
    }
}
