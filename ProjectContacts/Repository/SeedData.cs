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

                context.Projects.AddRange(
                    new Project { Title = "New Skyscraper" },
                    new Project { Title = "Block of units revovation" },
                    new Project { Title = "Shopping centre" },
                    new Project { Title = "New Restaurant" }
                );

                context.Contacts.AddRange(
                    new Contact { Name = "Fred Harris", Email = "f.harris@studabaker.com", Phone = "04 08734348" },
                    new Contact { Name = "Ming the Merciless", Email = "ming97@planet.com", Phone = "01 4364646" },
                    new Contact { Name = "Batman Jr", Email = "junior@batcave.hell", Phone = "02 74739434" },
                    new Contact { Name = "Ronaldinho", Email = "soccer.star@brazil.com", Phone = "9100 97343" },
                    new Contact { Name = "Clare Bakerson", Email = "clare@bakerson.inc", Phone = "+44 098734983" }
);
                context.SaveChanges();
            }
        }
    }
}
