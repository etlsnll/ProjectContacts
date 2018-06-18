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
    [Route("api/Contact")]
    public class ContactController : Controller
    {
        private IContactRepository _contactRepository;
        private readonly ILogger _logger;

        public ContactController(IContactRepository contactRepository,
                                  ILogger<ContactController> logger)
        {
            _contactRepository = contactRepository;
            _logger = logger;
        }


        // GET: api/Contact/Count
        [HttpGet("[action]")]
        public int Count()
        {
            var num = _contactRepository.AllContactsCount;
            return num;
        }

        /// <summary>
        /// GET: api/Contact/List (server side paged results)
        /// </summary>
        /// <param name="pageNum">Page number to get</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of contact classe instances</returns>
        [HttpGet("[action]")]
        public IEnumerable<Contact> List(int pageNum, int pageSize)
        {
            if (pageNum < 1)
                throw new ArgumentException("Value must be greater than 0", nameof(pageNum));
            if (pageSize < 1)
                throw new ArgumentException("Value must be greater than 0", nameof(pageSize));

            return _contactRepository.GetContacts(pageNum, pageSize);
        }

        /// <summary>
        /// DELETE: api/Contact/Delete/4
        /// </summary>
        /// <param name="id">ID of contact to delete</param>
        /// <returns>True if successful, false if not found</returns>
        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _contactRepository.DeleteContact(id);
            if (!result)
            {
                _logger.LogError($"Contact with ID {id} not found in DB", id);
                return NotFound();
            }

            return Ok(result);
        }
    }
}