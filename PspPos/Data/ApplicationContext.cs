using PspPos.Models;
using Microsoft.EntityFrameworkCore;

namespace PspPos.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {

    }

    public DbSet<Sample> Samples => Set<Sample>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Item> Items => Set<Item>();

    private HashSet<Guid>? _availableCompanies = null; 

    public async Task<HashSet<Guid>> GetAvailableCompanies()
    {
        if(_availableCompanies is null)
        {
            var companies = await Companies.ToListAsync();
            _availableCompanies = companies.Select(c => c.Id).ToHashSet()!;
        }

        return _availableCompanies;
    }

    public async Task<bool> CheckIfCompanyExists(Guid id)
    {
        var companies = await GetAvailableCompanies();
        if (companies.Contains(id))
            return true;

        return false;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Why were these gone??
        modelBuilder.Entity<Sample>().ToTable("Samples");
        modelBuilder.Entity<Company>().ToTable("Companies");
        modelBuilder.Entity<Item>().ToTable("Items");
        modelBuilder.Entity<Item>().ToTable("Orders");
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<User>()
            .HasOne(u => u.Company)
            .WithMany()
            .HasForeignKey(u => u.CompanyId);
    }
}
