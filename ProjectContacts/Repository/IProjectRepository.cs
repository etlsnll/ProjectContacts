using System.Collections.Generic;
using ProjectContacts.Models;

namespace ProjectContacts.Repository
{
    public interface IProjectRepository
    {
        int AllProjectsCount { get; }

        IEnumerable<Project> GetProjects(int pageNum, int pageSize);

        bool DeleteProject(int id);

        int AddProject(Project project);
    }
}