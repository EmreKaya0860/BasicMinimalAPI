using Microsoft.EntityFrameworkCore;
using SampleWebService.Models;

namespace SampleWebService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Page> Pages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.Property(e => e.StartDate).HasColumnType("timestamp without time zone");
                entity.Property(e => e.EndDate).HasColumnType("timestamp without time zone");
            });
        }
    }
}
