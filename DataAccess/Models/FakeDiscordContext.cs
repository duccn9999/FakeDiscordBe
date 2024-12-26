using Microsoft.EntityFrameworkCore;

namespace DataAccesses.Models
{
    public class FakeDiscordContext : DbContext
    {
        public FakeDiscordContext(DbContextOptions<FakeDiscordContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<GroupChatParticipation> Participations { get; set; }
        public DbSet<GroupMessage> GroupMsgs { get; set; }
        public DbSet<PrivateMessage> PrivateMsgs { get; set; }
    }
}
