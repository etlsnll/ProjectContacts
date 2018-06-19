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

            _logger.LogInformation($"Contact deleted - ID: {id}", id);

            return Ok(result);
        }


        // GET: api/Contact/Add
        [HttpPost("[action]")]
        public int Add([FromBody]Contact contact)
        {
            if (String.IsNullOrWhiteSpace(contact.Name))
                throw new ArgumentException("Contact must have a title", "Name");
            if (String.IsNullOrWhiteSpace(contact.Email))
                throw new ArgumentException("Contact must have an email", "Email");
            if (String.IsNullOrWhiteSpace(contact.Phone))
                throw new ArgumentException("Contact must have a phone number", "Phone");

            try
            {
                var id = _contactRepository.AddContact(contact);
                _logger.LogInformation("Contact added: {0}, ID: {1}", contact.Name, contact.ContactId);
                return id;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding contact: " + contact.Name);
                return 0;
            }
        }
    }
}