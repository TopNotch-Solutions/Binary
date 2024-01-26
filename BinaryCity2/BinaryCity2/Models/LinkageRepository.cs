using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BinaryCity2.Models
{
    public class LinkageRepository : ILinkageRepository
    {
        private readonly AppDbContext appDbContext;
        public LinkageRepository(AppDbContext appDbContext) 
        {
            this.appDbContext = appDbContext;
        }
        public string ProcessString(string input)
        {
            
            string[] parts = input.Split(' ');

           
            if (parts.Length >= 3)
            {

                string result = $"{char.ToUpper(parts[0][0])}{parts[1][0]}{parts[2][0]}{GenerateUniqueNumbers()}";
                return result;
            }
            else if (parts.Length <3 && parts[0].Length >2)
            {

                string result = $"{parts[0].Substring(0, 3).ToUpper()}{GenerateUniqueNumbers()}";
                return result;

            }
            else {
                var generated = GenerateRandomCharacters(1);
                string paddedString = input.PadRight(3, ' ').Substring(0, 3);
                paddedString = paddedString.Replace(" ", generated);
                string result = $"{paddedString.ToUpper()}{GenerateUniqueNumbers()}";
                return result;
            }
        }
        public static string GenerateRandomCharacters(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private string GenerateUniqueNumbers()
        {
            Random random = new Random();
            return $"{random.Next(10)}{random.Next(10)}{random.Next(10)}";
        }
        async Task<Client> ILinkageRepository.AddClient(Client client)
        {
            var clientsCode1 = ProcessString(client.ClientName);
            Client newClient = new Client();
            newClient.ClientName = client.ClientName;
            newClient.ClientCode = clientsCode1;

            var result = await appDbContext.Clients.AddAsync(newClient);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        async Task<Contact> ILinkageRepository.AddContact(Contact contact)
        {
            var result = await appDbContext.Contacts.AddAsync(contact);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        async Task ILinkageRepository.DeleteClient(int id)
        {
            var result = await appDbContext.Clients.FirstOrDefaultAsync(x =>x.ClientId == id);
            if(result != null)
            {
                appDbContext.Clients.Remove(result);
                await appDbContext.SaveChangesAsync();
            }
        }

        async Task ILinkageRepository.DeleteContact(int id)
        {
            var result = await appDbContext.Contacts.FirstOrDefaultAsync(x => x.ContactId == id);
            if (result != null)
            {
                appDbContext.Contacts.Remove(result);
                await appDbContext.SaveChangesAsync();
            }
        }

        async Task<IEnumerable<Client>> ILinkageRepository.GetAllClients()
        {
            return await appDbContext.Clients.ToListAsync();
        }

        async Task<IEnumerable<Contact>> ILinkageRepository.GetAllClientsLinked(int clientId)
        {
            IQueryable<Contact> query = appDbContext.Contacts;

            var result = await appDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);

            if (result != null)
            {
                var links = await appDbContext.ClientContactsLinks
                    .Where(x => x.ClientId == clientId)
                    .ToListAsync();

                if (links.Any())
                {
                    var contactIds = links.Select(link => link.ContactId);
                    query = query.Where(x => contactIds.Contains(x.ContactId));
                }
            }

            return await query.ToListAsync();
        }
        async Task<IEnumerable<Contact>> ILinkageRepository.GetAllContacts()
        {
            return await appDbContext.Contacts.ToListAsync();
        }

        async Task<Client> ILinkageRepository.GetClientById(int id)
        {
            return await appDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == id);
        }

        async Task<Client> ILinkageRepository.GetClientByCode(string code)
        {
            return await appDbContext.Clients.FirstOrDefaultAsync(x => x.ClientCode == code);
        }

        async Task<Contact> ILinkageRepository.GetContactById(int id)
        {
            return await appDbContext.Contacts.FirstOrDefaultAsync(x => x.ContactId == id);
        }

        async Task<ClientContactLink> ILinkageRepository.Link(ClientContactLink clientContactLink)
        {
            var result = await appDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientContactLink.ClientId);
            if(result != null)
            {
                var re = await appDbContext.Contacts.FirstOrDefaultAsync(x => x.ContactId == clientContactLink.ContactId);
                if(re != null)
                {
                    var r = await appDbContext.ClientContactsLinks.AddAsync(clientContactLink);
                    await appDbContext.SaveChangesAsync();
                    return r.Entity;
                }
            }
            return null;
        }

        async Task ILinkageRepository.UnLink(int clientId, int contactId)
        {
            var result = await appDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (result != null)
            {
                var re = await appDbContext.Contacts.FirstOrDefaultAsync(x => x.ContactId == contactId);
                if (re != null)
                {
                    var r = await appDbContext.ClientContactsLinks.FirstOrDefaultAsync(x => x.ClientId == clientId && x.ContactId == contactId);
                    if(r != null)
                    {
                         appDbContext.ClientContactsLinks.Remove(r);
                        await appDbContext.SaveChangesAsync ();
                    }
                }
            }
        
        }

        async Task<Contact> ILinkageRepository.UpdateContact(Contact contact)
        {
            var result = await appDbContext.Contacts.FirstOrDefaultAsync(x => x.ContactId == contact.ContactId);
            if (result != null)
            {
                result.SurName = contact.SurName;
                result.Name = contact.Name;
                result.Email = contact.Email;

                await appDbContext.SaveChangesAsync();
            }
            return null;
        }

        async Task<Contact> ILinkageRepository.GetContactByEmail(string email)
        {
            return await appDbContext.Contacts.FirstOrDefaultAsync(x => x.Email == email);
        }

        async Task<ClientContactLink> ILinkageRepository.GetClientContactLinkById(int id)
        {
            return await appDbContext.ClientContactsLinks.FirstOrDefaultAsync(x => x.ClientContactLinkId == id);
        }

        async Task<int> ILinkageRepository.GetCountClientsLinked(int clientId)
        {
                   return await appDbContext.ClientContactsLinks.CountAsync(x => x.ClientId == clientId);
        }
    }
}
