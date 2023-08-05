using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Database
{
    public class WebAppDBContext : DbContext
    {
        public WebAppDBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Laptop>().HasKey(l => l.Number);
            modelBuilder.Entity<Store>().HasKey(s => s.StoreNumber);
        }

        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Laptop> Laptops { get; set; } = null!;
        public DbSet<Store> Stores { get; set; } = null!;
        public DbSet<StoreLaptopStock> StoreLaptopStocks { get; set; } = null!;
    }
}
