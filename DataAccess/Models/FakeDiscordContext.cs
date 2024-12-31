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
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PrivateMessage> PrivateMsgs { get; set; }
        public DbSet<Participation> Participations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-many relationship configuration
            modelBuilder.Entity<User>()
                .HasMany(x => x.GroupChats)
                .WithMany(x => x.Users)
                .UsingEntity<Participation>(
                j => j.Property(e => e.DateJoined).HasDefaultValueSql("CURRENT_TIMESTAMP"));
        }
    }
}
