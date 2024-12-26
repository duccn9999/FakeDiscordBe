namespace DataAccesses.DTOs.Users
{
    public class UserParticipationDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<ParticipateInGroupsDTO>? Participations { get; set; }
    }
}
