using Microsoft.EntityFrameworkCore;

namespace BrewerPredictorApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //public DbSet<ContentTypeEntity> ContentTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your entity relationships here
            // For example:
            // modelBuilder.Entity<YourEntity>()
            //     .HasMany(e => e.RelatedEntities)
            //     .WithOne(e => e.YourEntity)
            //     .HasForeignKey(e => e.YourEntityId);
        }
    }
}
