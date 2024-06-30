using Microservice.Core.Config;
using Microservice.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Microservice.Persistence.Database;

public static class DependencyInjection
{

    public static void AddDatabaseServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, EventPublisher>();
        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

        if (true)
        {
            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.UseInMemoryDatabase("InMemoryDb");
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            });
        }
        else
        {
            string? connectionString = configuration.GetConnectionString("Database");
            services.AddDbContext<AppDbContext>((options) =>
            {
                options.UseSqlServer(connectionString);
            });
        }
    }
}

