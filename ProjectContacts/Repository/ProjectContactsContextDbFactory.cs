using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProjectContacts.Repository
{
    public class ProjectContactsContextDbFactory : IDesignTimeDbContextFactory<ProjectContactsContext>
    {
        ProjectContactsContext IDesignTimeDbContextFactory<ProjectContactsContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectContactsContext>();
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer<ProjectContactsContext>(configuration.GetConnectionString("ProjectContactsDB"));

            return new ProjectContactsContext(optionsBuilder.Options);
        }
    }
}


