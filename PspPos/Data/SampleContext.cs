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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sample>().ToTable("Samples");
        }
    }
}
