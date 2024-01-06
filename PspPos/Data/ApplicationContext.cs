using PspPos.Models;
using Microsoft.EntityFrameworkCore;

namespace PspPos.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<Sample> Samples => Set<Sample>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Item> Items => Set<Item>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sample>().ToTable("Samples");
            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<Item>().ToTable("Items");
        }
    }
}
