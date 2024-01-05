using PspPos.Models;
using Microsoft.EntityFrameworkCore;

namespace PspPos.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    { }

    public DbSet<Sample> Samples => Set<Sample>();
    public DbSet<Company> Companies => Set<Company>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sample>().ToTable("Samples");
        modelBuilder.Entity<Company>().ToTable("Companies");
    }
}
