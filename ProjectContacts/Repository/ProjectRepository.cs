using System.Collections.Generic;
using System.Linq;
using ProjectContacts.Models;

namespace ProjectContacts.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private ProjectContactsContext _dbContext;

        public ProjectRepository(ProjectContactsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AllProjectsCount
        {
            get
            {
                var result = _dbContext.Projects.Count();
                return result;
            }
        }

        public IEnumerable<Project> GetProjects(int pageNum, int pageSize)
        {
            var result = _dbContext.Projects.OrderBy(p => p.Title)
                                             .Skip((pageNum - 1) * pageSize)
                                             .Take(pageSize);
            return result;
        }

        public bool DeleteProject(int id)
        {
            var pr = _dbContext.Projects.FirstOrDefault(x => x.ProjectId == id);
            if (pr != null)
            {
                // Remove all contacts for the project first...
                _dbContext.ProjectContacts.Where(x => x.ProjectId == id).ToList().ForEach(t => _dbContext.Remove(t));
                _dbContext.Remove(pr);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
