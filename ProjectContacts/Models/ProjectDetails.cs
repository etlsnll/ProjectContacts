using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectContacts.Models
{
    public class ProjectDetails : Project
    {
        public IEnumerable<Contact> Participants { get; set; }
    }
}
