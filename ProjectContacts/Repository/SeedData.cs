using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectContacts.Models;

namespace ProjectContacts.Repository
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ProjectContactsContext(
                serviceProvider.GetRequiredService<DbContextOptions<ProjectContactsContext>>()))
            {
                if (context.Projects.Any() || context.Contacts.Any())
                    return;   // DB has been seeded

                // Add some sample data for testing:
                context.Projects.AddRange(
                    new Project { Title = "New Skyscraper", Created = DateTime.Now },
                    new Project { Title = "Block of units revovation", Created = DateTime.Now },
                    new Project { Title = "Shopping centre", Created = DateTime.Now },
                    new Project { Title = "New Restaurant", Created = DateTime.Now }
                );
                for (var i = 1; i < 13; i++)
                    context.Projects.Add(new Project { Title = $"Project_{i}", Created = DateTime.Now.AddDays(-i) });

                context.Contacts.AddRange(
                    new Contact { Name = "Fred Harris", Email = "f.harris@studabaker.com", Phone = "04 08734348" },
                    new Contact { Name = "Ming the Merciless", Email = "ming97@planet.com", Phone = "01 4364646" },
                    new Contact { Name = "Batman Jr", Email = "junior@batcave.hell", Phone = "02 74739434" },
                    new Contact { Name = "Ronaldinho", Email = "soccer.star@brazil.com", Phone = "9100 97343" },
                    new Contact { Name = "Clare Bakerson", Email = "clare@bakerson.inc", Phone = "+44 098734983" }
                );
                for (var i = 1; i < 25; i++)
                    context.Contacts.Add(new Contact { Name = $"Hench Man #{i} ", Email = $"user.{i}@copypaste.com", Phone = RandomDigits(10) });

                context.SaveChanges();
            }
        }

        private static string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
    }
}
