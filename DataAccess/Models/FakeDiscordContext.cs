﻿using Microsoft.EntityFrameworkCore;

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
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<AllowedRole> AllowedRoles { get; set; }
        public DbSet<AllowedUser> AllowedUsers { get; set; }
        public DbSet<UserFriend> UserFriends { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PrivateMessageAttachment> PrivateMessageAttachments { get; set; }
        public DbSet<MessageAttachment> MessageAttachments { get; set; }
        public DbSet<MentionUser> MentionUsers { get; set; }
        public DbSet<LastSeenMessage> LastSeenMessages { get; set; }
        public DbSet<EmailToken> EmailTokens { get; set; }
        public DbSet<GroupChatBlackList> GroupChatBlackLists { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<SuspendUser> SuspendUsers { get; set; }
        public DbSet<SuspendGroupChat> SuspendGroupChats { get; set; }
        public DbSet<SystemNotification> SystemNotifications { get; set; }
        public DbSet<BlockedUser> BlockedUsers { get; set; }
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
        }
    }
}
