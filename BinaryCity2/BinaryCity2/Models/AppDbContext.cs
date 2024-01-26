using Microsoft.EntityFrameworkCore;

namespace BinaryCity2.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ClientContactLink> ClientContactsLinks { get; set; }
    }
    
}
