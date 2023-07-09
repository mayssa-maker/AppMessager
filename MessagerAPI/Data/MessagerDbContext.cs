using Microsoft.EntityFrameworkCore;
using MessagerAPI.Models;
using Microsoft.Extensions.Options;

namespace MessageAppBack.Data
{
    public class MessagerDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public MessagerDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DbSet<Userr> Users { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder Options)
        {
            Options.UseSqlite(Configuration.GetConnectionString("webApiDb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageModel>()
                   .HasOne(m => m.Conversation)
                   .WithMany(c => c.Messages)
                   .HasForeignKey(m => m.ConversationId);

            modelBuilder.Entity<Conversation>()
          .HasOne(c => c.User1)
          .WithMany()
          .HasForeignKey(c => c.User1Id);

            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.User2)
                .WithMany()
                .HasForeignKey(c => c.User2Id);


        }
    }
}