using MessagerAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace MessagerAPI.Data
{
    public class MessagerDbContext : IdentityDbContext<IdentityUser>
{   protected readonly IConfiguration Configuration;
   

    public MessagerDbContext(IConfiguration configuration)
       
        {
            Configuration = configuration;
    }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder Options)
    {
       
    
                // Configure the database connection here
                Options.UseSqlite(Configuration.GetConnectionString("webApiDb"));
    
}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the relationships between Conversations and Messages
        modelBuilder.Entity<Conversation>()
            .HasMany(c => c.Messages)
            .WithOne(m => m.Conversation)
            .HasForeignKey(m => m.ConversationId);
    }
}

 
 
}
 
 
 