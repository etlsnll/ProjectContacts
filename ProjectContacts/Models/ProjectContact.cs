using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectContacts.Models
{
    // Mapping for projects to contacts:
    public class ProjectContact
    {
        public int ProjectContactId { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}
