using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Services;
using Microsoft.EntityFrameworkCore;
using PspPos.Repositories;

namespace PspPos.Commons;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationContext>(options =>
           options.UseSqlServer(defaultConnectionString));


// For some reason this made some problems for migrations. So just use dotnet ef database update and dotnet ef migrations add NameOfMigration
        // using (var scope = services.BuildServiceProvider().CreateScope())
        // {
        //     var dataContext = scope.ServiceProvider.GetRequiredService<SampleContext>();
        //     dataContext.Database.Migrate();
        // }
// Section ends here

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<ISampleService, SampleService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAppointmentsService, AppointmentsService>();
        services.AddScoped<IServiceRepository, ServiceRepository>();

        return services;
    }
}
