using System.Collections.Generic;
using ProjectContacts.Models;

namespace ProjectContacts.Repository
{
    public interface IContactRepository
    {
        int AllContactsCount { get; }

        IEnumerable<Contact> GetContacts(int pageNum, int pageSize);

        bool DeleteContact(int id);
    }
}