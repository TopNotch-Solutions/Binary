namespace BinaryCity2.Models
{
    public interface ILinkageRepository
    {
        // client task
        Task<Client> AddClient(Client client);
        Task DeleteClient(int id);
        Task<IEnumerable<Client>> GetAllClients();
        Task<Client> GetClientByCode(string code);
        Task<Client> GetClientById(int id);

        //contact task
        Task<Contact> AddContact(Contact contact);
        Task<Contact> UpdateContact(Contact contact);
        Task<IEnumerable<Contact>> GetAllContacts();
        Task<Contact> GetContactById(int id);
        Task<Contact> GetContactByEmail(string email);
        Task DeleteContact(int id);

        //linked task
        Task<ClientContactLink> Link(ClientContactLink clientContactLink);
        Task<ClientContactLink> GetClientContactLinkById(int id);
        Task<IEnumerable<Contact>> GetAllClientsLinked(int clientId);
        Task UnLink(int clientId, int contactId);
        Task<int> GetCountClientsLinked(int clientId);
    }
}
