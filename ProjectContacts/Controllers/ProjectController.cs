using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectContacts.Models;
using ProjectContacts.Repository;

namespace ProjectContacts.Controllers
{
    [Produces("application/json")]
    [Route("api/Project")]
    public class ProjectController : Controller
    {
        private IProjectRepository _projectRepository;
        private readonly ILogger _logger;

        public ProjectController(IProjectRepository projectRepository,
                                  ILogger<ProjectController> logger)
        {
            _projectRepository = projectRepository;
            _logger = logger;
        }


        // GET: api/Project/Count
        [HttpGet("[action]")]
        public int Count()
        {
            var num = _projectRepository.AllProjectsCount;
            return num;
        }

        /// <summary>
        /// GET: api/Project/List (server side paged results)
        /// </summary>
        /// <param name="pageNum">Page number to get</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of project classes</returns>
        [HttpGet("[action]")]
        public IEnumerable<Project> List(int pageNum, int pageSize)
        {
            if (pageNum < 1)
                throw new ArgumentException("Value must be greater than 0", nameof(pageNum));
            if (pageSize < 1)
                throw new ArgumentException("Value must be greater than 0", nameof(pageSize));

            return _projectRepository.GetProjects(pageNum, pageSize);
        }

        /// <summary>
        /// DELETE: api/Project/Delete/4
        /// </summary>
        /// <param name="id">ID of project to delete</param>
        /// <returns>True if successful, false if not found</returns>
        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _projectRepository.DeleteProject(id);
            if (!result)
            {
                _logger.LogError($"Project with ID {id} not found in DB", id);
                return NotFound();
            }

            _logger.LogInformation($"Project deleted - ID: {id}", id);

            return Ok(result);
        }

        // GET: api/Project/Add
        [HttpPost("[action]")]
        public int Add([FromBody]Project project)
        {
            if (String.IsNullOrWhiteSpace(project.Title))
                throw new ArgumentException("Project must have a title", "Title");

            try
            {
                var id = _projectRepository.AddProject(project);
                _logger.LogInformation("Project added: {0}, ID: {1}", project.Title, project.ProjectId);
                return id;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding project: " + project.Title);
                return 0;
            }
        }
    }
}