using Microsoft.EntityFrameworkCore;
using ProjectContacts.Models;

namespace ProjectContacts.Repository
{
    public class ProjectContactsContext : DbContext
    {
        public ProjectContactsContext(DbContextOptions<ProjectContactsContext> options) : base(options)
        { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ProjectContact> ProjectContacts { get; set; }
    }
}
