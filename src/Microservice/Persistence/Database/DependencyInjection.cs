using Microservice.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Microservice.Persistence.Database;

public static class DependencyInjection
{

    public static void AddDatabaseServices(this IServiceCollection services)
    {
        services.AddScoped<ISaveChangesInterceptor, EventPublisher>();
        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseInMemoryDatabase("InMemoryDb");

            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });
    }
}

