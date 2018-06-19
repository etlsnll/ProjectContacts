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

        // GET: api/Project/3
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _projectRepository.GetProject(id);
            if (result == null)
            {
                _logger.LogError($"Project with ID {id} not found in DB", id);
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// GET: api/Project/2/SearchContacts
        /// </summary>
        /// <param name="id">ID of projectt</param>
        /// <param name="srchTerm">search term</param>
        /// <returns>List of project classes</returns>
        [HttpGet("{id}/[action]")]
        public IEnumerable<Contact> SearchContacts(int id, string srchTerm)
        {
            return _projectRepository.SearchContacts(id, srchTerm, 50);
        }

        /// <summary>
        /// DELETE: api/Project/Delete/4
        /// </summary>
        /// <param name="id">ID of project to delete</param>
        /// <returns>True if successful, NotFound not found</returns>
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

        /// <summary>
        /// DELETE: api/Project/7/DeleteParticipant/96
        /// </summary>
        /// <param name="id">ID of project</param>
        /// <param name="contactId">ID of project to delete</param>
        /// <returns>True if successful, NotFound not found</returns>
        [HttpDelete("{id}/[action]/{contactId}")]
        public IActionResult DeleteParticipant(int id, int contactId)
        {
            var result = _projectRepository.DeleteProjectParticipant(id, contactId);
            if (result == null)
            {
                _logger.LogError($"Project with ID {id} and/or contact with ID {contactId} not found in DB", id, contactId);
                return NotFound();
            }

            _logger.LogInformation($"Project participant ID {contactId} deleted for project ID: {id}", contactId, id);

            return Ok(result);
        }

        // POST: api/Project/Add
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

        /// <summary>
        /// PUT: api/Project/4 - update basic properties of project
        /// </summary>
        /// <param name="id">ID of project to update</param>
        /// <returns>Updated object if successful, NotFound if not found</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateById(int id, [FromBody]ProjectDetails project)
        {
            if (String.IsNullOrWhiteSpace(project.Title))
                throw new ArgumentException("Project must have a title", "Title");

            var result = _projectRepository.UpdateProject(id, project);
            if (result == null)
            {
                _logger.LogError($"Project with ID {id} not found in DB", id);
                return NotFound();
            }

            _logger.LogInformation($"Project updated - ID: {id}", id);

            return Ok(result);
        }

        /// <summary>
        /// PUT: api/Project/4/AddParticipant - add Contact
        /// </summary>
        /// <param name="id">ID of project to update</param>
        /// <param name="c">contact to add</param>
        /// <returns>Updated list of participants</returns>
        [HttpPut("{id}/[action]")]
        public IActionResult AddParticipant(int id, [FromBody]Contact c)
        {
            var result = _projectRepository.AddParticipant(id, c.ContactId);
            if (result == null)
            {
                _logger.LogError($"Project with ID {id} not found in DB", id);
                return NotFound();
            }

            _logger.LogInformation($"Project updated - ID: {id}, Contact ID {c.ContactId} added.", id, c.ContactId);

            return Ok(result);
        }
    }
}