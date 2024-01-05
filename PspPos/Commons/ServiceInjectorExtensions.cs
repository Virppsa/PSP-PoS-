using PspPos.Data;
using Microsoft.EntityFrameworkCore;

namespace PspPos.Commons;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationContext>(options =>
           options.UseSqlServer(defaultConnectionString));
        services.AddDbContext<SampleContext>(options =>
           options.UseSqlServer(defaultConnectionString));


        // For some reason this made some problems for migrations. So just use dotnet ef database update and dotnet ef migrations add NameOfMigration
        // using (var scope = services.BuildServiceProvider().CreateScope())
        // {
        //     var dataContext = scope.ServiceProvider.GetRequiredService<SampleContext>();
        //     dataContext.Database.Migrate();
        // }
        // Section ends here

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
