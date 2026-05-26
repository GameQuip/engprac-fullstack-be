/*
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        // 1. The constructor passes configuration options up to the base DbContext class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 2. Define your tables using DbSet. 
        // This tells EF Core to map your 'BookableItem' class to a table called 'BookableItems'
        public DbSet<BookableItem> BookableItems { get; set; }

        // 3. Optional: Configure table rules (like unique constraints or seed data)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example: Make the item name required and limit length
            modelBuilder.Entity<BookableItem>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });
        }
    }
}
*/