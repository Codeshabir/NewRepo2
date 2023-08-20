using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplicationWebApi_Arooj.Model;

namespace WebApplicationWebApi_Arooj.Data
{
    public class HouseRentingDbContext : DbContext
    {
        public HouseRentingDbContext(DbContextOptions<HouseRentingDbContext> options)
            : base(options)
        {
        }

        public DbSet<House> Houses { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<House>(entity =>
            {
                
                entity.Property(e => e.Rent)
                    .HasColumnType("decimal(18, 2)"); // Adjust precision and scale as needed
            });

            // Other entity configurations
        }
    }

}
