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
        }
    }
}
