using DataAccesses.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // Check if any roles already exists
            if (!_context.Roles.Any())
            {
                // Add the roles
                _context.Roles.AddRange(new List<Role>
                {
                    new() {
                        RoleName = "Member",
                        Color = "#FFFFFF",
                        DateCreated = DateTime.UtcNow,
                        UserCreated = 0
                    },
                    new() {
                        RoleName = "Moderator",
                        Color = "#FFFFFF",
                        DateCreated = DateTime.UtcNow,
                        UserCreated = 0
                    }
                });
                _context.SaveChanges();
            }
            if (!_context.Permissions.Any())
            {
                _context.Permissions.AddRange(new List<Permission>
                {
                    new()
                    {
                        Name = "CanViewChannels",
                        Description = "Allows members to view channels by default (Excluding private channels).",
                    },
                    new()
                    {
                        Name = "CanManageChannels",
                        Description = "Allows members to create, edit or delete channels",
                    },
                    new()
                    {
                        Name = "CanManageRoles",
                        Description = "Allows members to create new roles and edit or delete roles lower than their highest role. Also allows members to change permissions " +
                        "of individual channels that they have access to",
                    },
                    new()
                    {
                        Name = "CanCreateInvites",
                        Description = "Allows members to invite new people to this group chat",
                    },
                    new()
                    {
                        Name = "CanBanMembers",
                        Description = "Allows members to permanently ban and delete the message history of other members from this group chat",
                    },
                    new()
                    {
                        Name = "CanTimeOutMembers",
                        Description = "When you put a user in time-out they will not be able to send messages in chat, reply, react,....",
                    },
                    new()
                    {
                        Name = "CanSendMessages",
                        Description = "Allows members to send messages in text channels",
                    },
                    new()
                    {
                        Name = "CanManageMessages",
                        Description = "Allows members to send messages in text channels",
                    },
                });
                _context.SaveChanges();
            }
        }
    }
}
