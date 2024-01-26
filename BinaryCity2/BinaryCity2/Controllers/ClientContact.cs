using BinaryCity2.Models;
using Microsoft.AspNetCore.Mvc;

namespace BinaryCity2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientContact : ControllerBase
    {
        private readonly ILinkageRepository linkageRepository;
        public ClientContact(ILinkageRepository linkageRepository)
        {
           this.linkageRepository = linkageRepository;
        }
        [Route("CreateClient")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<Client>> CreateClient(Client client)
        {
            try
            {
                if (client == null)
                {
                    return BadRequest();
                }
                var result = await linkageRepository.GetClientByCode(client.ClientCode);
                if (result != null)
                {

                    return BadRequest("Client already exist");
                }
                var createClient = await linkageRepository.AddClient(client);
                return CreatedAtAction(nameof(linkageRepository.GetClientById), new { id = client.ClientId }, createClient);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Route("CreateContact")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<Contact>> CreateContact(Contact contact)
        {
            try
            {
                if (contact == null)
                {
                    return BadRequest();
                }
                var result = await linkageRepository.GetContactByEmail(contact.Email);
                if (result != null)
                {

                    return BadRequest("Client already exist");
                }
                var createContact = await linkageRepository.AddContact(contact);
                return CreatedAtAction(nameof(linkageRepository.GetContactById), new { id = contact.ContactId }, createContact);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Route("DeleteClient")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> DeleteClient(int id)
        {
            try
            {
                var result = await linkageRepository.GetClientById(id);
                if (result == null)
                {
                    return NotFound();
                }
                await linkageRepository.DeleteClient(id);
                return Ok($"Client {id} has been deleted!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Route("DeleteContact")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> DeleteContact(int id)
        {
            try
            {
                var result = await linkageRepository.GetContactById(id);
                if (result == null)
                {
                    return NotFound();
                }
                await linkageRepository.DeleteContact(id);
                return Ok($"Client {id} has been deleted!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Route("AllClients")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> GetAllClients()
        {
            try
            {
                return Ok(await linkageRepository.GetAllClients());

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Route("GetClientsLinked")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Client>> GetClientsLinked(int id)
        {
            try
            {
                var result = await linkageRepository.GetAllClientsLinked(id);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Route("AllContacts")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> GetAllContacts()
        {
            try
            {
                return Ok(await linkageRepository.GetAllContacts());

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Route("GetTotalContactLinked")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> GetTotalContactLinked(int clientId)
        {
            try
            {
                return Ok(await linkageRepository.GetCountClientsLinked(clientId));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [Route("Linking")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<ClientContactLink>> Linking(ClientContactLink clientContactLink)
        {
            try
            {
                if (clientContactLink == null)
                {
                    return BadRequest();
                }
               
                var createContact = await linkageRepository.Link(clientContactLink);
                return CreatedAtAction(nameof(linkageRepository.GetClientContactLinkById), new { id = clientContactLink.ClientContactLinkId }, clientContactLink);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Route("UnLink")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> UnLink(int clientId, int contactId)
        {
            try
            {
                var result = await linkageRepository.GetContactById(contactId);
                if (result == null)
                {
                    return NotFound();
                }
                await linkageRepository.UnLink(clientId, contactId);
                return Ok($"Contact {contactId} has been unlined!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
