using BrewerPredictorApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BrewerPredictorApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<NLCentralStanding> NLCentralStandings { get; set; }
        public DbSet<Prediction> Predictions { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure NLCentralStanding entity
            modelBuilder.Entity<NLCentralStanding>()
                .ToTable("NLCentralStandings");

            // Configure Prediction entity
            modelBuilder.Entity<Prediction>()
                .ToTable("Prediction");

            // Configure Message entity
            modelBuilder.Entity<Message>()
                .ToTable("Messages");
            
            modelBuilder.Entity<Message>()
                .Property(m => m.MessageText)
                .HasColumnName("Message");
                
            // Configure HasResponded column
            modelBuilder.Entity<Message>()
                .Property(m => m.HasResponded)
                .HasColumnName("HasResponded")
                .HasDefaultValue(false);
        }
    }
}
