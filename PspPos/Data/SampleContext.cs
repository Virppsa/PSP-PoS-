using PspPos.Models;
using Microsoft.EntityFrameworkCore;

namespace PspPos.Data
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions<SampleContext> options) : base(options)
        {

        }

        public DbSet<Sample> Samples => Set<Sample>();
        public DbSet<Company> Companies => Set<Company>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sample>().ToTable("Samples");
            modelBuilder.Entity<Company>().ToTable("Companies");
        }
    }
}
