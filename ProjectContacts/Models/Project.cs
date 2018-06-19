using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectContacts.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }

        // Add other properties of the project entity here as required
    }
}
