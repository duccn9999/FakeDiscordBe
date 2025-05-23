using DataAccesses.Models;

namespace DataAccesses.Seeds
{
    public class DataSeeder
    {
        private readonly FakeDiscordContext _context;

        public DataSeeder(FakeDiscordContext context)
        {
            _context = context;
        }

        public void SeedRoles()
        {
            if (!_context.Permissions.Any())
            {
                _context.Permissions.AddRange(new List<Permission>
                {
                    new()
                    {
                        //PermissionId = 1,
                        Value = "CanManageChannels",
                        DisplayName="Manage channels",
                        Description = "Allows members to create, edit or delete channels",
                    },
                    new()
                    {
                        //PermissionId = 2,
                        Value = "CanManageRoles",
                        DisplayName= "Manage roles",
                        Description = "Allows members to create new roles and edit or delete roles lower than their highest role. Also allows members to change permissions " +
                        "of individual channels that they have access to",
                    },
                    new()
                    {
                        //PermissionId = 3,
                        Value = "CanCreateInvites",
                        DisplayName = "Create invites",
                        Description = "Allows members to invite new people to this group chat",
                    },
                    new()
                    {
                        //PermissionId = 4,
                        Value = "CanSendMessages",
                        DisplayName= "Send messages",
                        Description = "Allows members to send messages in text channels",
                    },
                    new()
                    {   
                        //PermissionId = 5,  
                        Value = "CanManageMessages",
                        DisplayName= "Manage messages",
                        Description = "Allows members to delete messages by other members",
                    },
                    new()
                    {   
                        //PermissionId = 6,
                        Value = "CanEditGroupChat",
                        DisplayName= "Edit group chat",
                        Description = "Allows members to edit the group chat",
                    },
                    new()
                    {   
                        //PermissionId = 7,
                        Value = "CanManageMembers",
                        DisplayName= "Manage members",
                        Description = "Allows members to manage other members in the group chat (ban, timeout, kick,...)",
                    }
                });
                _context.SaveChanges();
            }
            if (!_context.SuperAdmins.Any())
            {
                _context.SuperAdmins.Add(new SuperAdmin
                {
                    Username = "admin",
                    Password = "admin123", // Replace with a hashed password in production
                    Email = "bbbvvvv442@gmail.com",
                    DateCreated = DateTime.Now
                });
                _context.SaveChanges();
            }
        }
    }
}
