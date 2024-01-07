using PspPos.Models;
using Microsoft.EntityFrameworkCore;

namespace PspPos.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {

    }

    public ApplicationContext Instance => this;
    public DbSet<Sample> Samples => Set<Sample>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<ItemOption> ItemOptions => Set<ItemOption>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();


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
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<User>()
            .HasOne(u => u.Company)
            .WithMany()
            .HasForeignKey(u => u.CompanyId);

        modelBuilder.Entity<Store>().ToTable("Stores");
        modelBuilder.Entity<Store>()
            .HasOne(u => u.Company)
            .WithMany()
            .HasForeignKey(u => u.CompanyId);
    }
}
