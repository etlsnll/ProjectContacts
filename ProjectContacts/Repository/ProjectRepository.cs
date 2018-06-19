using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
                _dbContext.ProjectContacts.Where(x => x.ProjectId == id).ToList().ForEach(pc => _dbContext.Remove(pc));
                _dbContext.Remove(pr);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<Contact> DeleteProjectParticipant(int id, int contactId)
        {
            var pc = _dbContext.ProjectContacts.FirstOrDefault(x => x.ProjectId == id && x.ContactId == contactId);
            if (pc != null)
            {
                _dbContext.Remove(pc);
                _dbContext.SaveChanges();
                return _dbContext.ProjectContacts.Where(x => x.ProjectId == id).Select(x => x.Contact).ToList();
            }
            return null;
        }

        public int AddProject(Project project)
        {
            project.Created = DateTime.Now; // Should really use UtcNow here and adjust to system timezone after read from DB if had more time
            _dbContext.Projects.Add(project);
            _dbContext.SaveChanges();

            return project.ProjectId;
        }

        public ProjectDetails GetProject(int id)
        {
            var pr = _dbContext.Projects.FirstOrDefault(x => x.ProjectId == id);
            if (pr != null)
                return LoadProject(id, pr);

            return null;
        }

        public ProjectDetails UpdateProject(int id, ProjectDetails project)
        {
            var pr = _dbContext.Projects.FirstOrDefault(x => x.ProjectId == id);
            if (pr != null)
            {
                pr.Title = project.Title;
                _dbContext.SaveChanges();

                return LoadProject(id, pr);
            }
            return null;
        }

        public IEnumerable<Contact> SearchContacts(int id, string srchTerm, int max)
        {
            var pr = _dbContext.Projects.FirstOrDefault(x => x.ProjectId == id);
            if (pr != null)
            {
                var currentParticipants = _dbContext.ProjectContacts.Where(x => x.ProjectId == id).Select(x => x.ContactId);
                var result = _dbContext.Contacts
                               .Where(c => String.IsNullOrEmpty(srchTerm) || EF.Functions.Like(c.Name, "%" + srchTerm + "%"))
                               .Where(c => !currentParticipants.Contains(c.ContactId)) // Don't get ones we already have!
                               .OrderBy(c => c.Name)
                               .Take(max);
                return result;
            }
            return null;
        }

        public IEnumerable<Contact> AddParticipant(int id, int contactId)
        {
            var pr = _dbContext.Projects.FirstOrDefault(x => x.ProjectId == id);
            if (pr != null)
            {
                var currentParticipants = _dbContext.ProjectContacts.Where(x => x.ProjectId == id);
                if (!currentParticipants.Any(x => x.ContactId == contactId))
                {
                    // Add to list if not already there:
                    _dbContext.ProjectContacts.Add(new ProjectContact { ProjectId = id, ContactId = contactId });
                    _dbContext.SaveChanges();
                }

                return _dbContext.ProjectContacts.Where(x => x.ProjectId == id)
                                                 .Select(x => x.Contact)
                                                 .OrderBy(x => x.Name);
            }
            return null;
        }

        private ProjectDetails LoadProject(int id, Project pr)
        {
            return new ProjectDetails
            {
                ProjectId = pr.ProjectId,
                Title = pr.Title,
                Created = pr.Created,
                Participants = _dbContext.ProjectContacts.Where(x => x.ProjectId == id)
                                                         .Select(pc => pc.Contact)
                                                         .OrderBy(c => c.Name)
                                                         .ToList()
            };
        }
    }
}
