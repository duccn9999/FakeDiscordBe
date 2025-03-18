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
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserGroupChat> UserGroupChats { get; set; }
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
            .HasKey(gr => new { gr.GroupChatId, gr.RoleId }); // Composite primary key

            modelBuilder.Entity<GroupChatRole>()
            .HasOne(gr => gr.GroupChat)
            .WithMany(u => u.GroupChatRoles)
            .HasForeignKey(ur => ur.GroupChatId);

            modelBuilder.Entity<GroupChatRole>()
                .HasOne(gr => gr.Role)
                .WithMany(r => r.GroupChatRoles)
                .HasForeignKey(gr => gr.RoleId);


            modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(u => u.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);
            modelBuilder.Entity<RolePermission>()
        .HasKey(rp => new { rp.RoleId, rp.PermissionId });


            modelBuilder.Entity<UserGroupChat>()
            .HasOne(rp => rp.User)
            .WithMany(u => u.UserGroupChats)
            .HasForeignKey(rp => rp.UserId);

            modelBuilder.Entity<UserGroupChat>()
                .HasOne(rp => rp.GroupChat)
                .WithMany(r => r.UserGroupChats)
                .HasForeignKey(rp => rp.GroupChatId);

            modelBuilder.Entity<UserGroupChat>().HasKey(rp => new { rp.UserId, rp.GroupChatId });
        }

    }
}
