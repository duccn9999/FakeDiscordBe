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

        public DbSet<GroupChatRole> GroupChatRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId }); // Composite primary key

            modelBuilder.Entity<UserRole>()
                .Property(ur => ur.DateAdded)
                .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Auto-set DateAdded

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<GroupChatRole>()
            .HasKey(ur => new { ur.GroupChatId, ur.RoleId }); // Composite primary key

            modelBuilder.Entity<GroupChatRole>()
            .HasOne(ur => ur.GroupChat)
            .WithMany(u => u.GroupChatRoles)
            .HasForeignKey(ur => ur.GroupChatId);

            modelBuilder.Entity<GroupChatRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.GroupChatRoles)
                .HasForeignKey(ur => ur.RoleId);
        }

    }
}
