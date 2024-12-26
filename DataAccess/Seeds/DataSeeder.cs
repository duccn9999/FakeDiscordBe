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
            // Check if the "Moderator" role already exists
            if (!_context.Roles.Any(r => r.RoleName == "Moderator"))
            {
                // Add the "Moderator" role
                _context.Roles.Add(new Role
                {
                    RoleName = "Moderator",
                    Color = "#FFFFFF",
                    DateCreated = DateTime.UtcNow,
                    UserCreated = 0
                });

                _context.SaveChanges();
            }
        }
    }
}
