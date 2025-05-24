namespace DataAccesses.DTOs.PaginationModels
{
    public class GroupChatPaginationDTO
    {
        public int GroupChatId { get; set; }
        public string Name { get; set; }
        public string? CoverImage { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserCreated { get; set; }
        public bool IsActive { get; set; }
    }
    public class GroupChats
    {
        public List<GroupChatPaginationDTO> Data { get; set; }
        public int Pages { get; set; }
    }
}
