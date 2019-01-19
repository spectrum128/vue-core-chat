using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StChat.Entities;

namespace StChat.Helpers
{
    public class DataContext : DbContext
    {
        
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> ChatUsers { get; set; }
        public DbSet<ContactRequest> ContactRequests { get; set; }
        public DbSet<AcceptedContact> AcceptedContacts { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        
    }
}