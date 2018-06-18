using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectContacts.Models;

namespace ProjectContacts.Repository
{
    public class ContactRepository : IContactRepository
    {
        private ProjectContactsContext _dbContext;

        public ContactRepository(ProjectContactsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AllContactsCount
        {
            get
            {
                var result = _dbContext.Contacts.Count();
                return result;
            }
        }

        public IEnumerable<Contact> GetContacts(int pageNum, int pageSize)
        {
            var result = _dbContext.Contacts.OrderBy(p => p.Name) // Extend to add ordering option in future...
                                             .Skip((pageNum - 1) * pageSize)
                                             .Take(pageSize);
            return result;
        }

        public bool DeleteContact(int id)
        {
            var c = _dbContext.Contacts.FirstOrDefault(x => x.ContactId == id);
            if (c != null)
            {
                // Remove all links to projects first...
                _dbContext.ProjectContacts.Where(x => x.ContactId == id).ToList().ForEach(t => _dbContext.Remove(t));
                _dbContext.Remove(c);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
