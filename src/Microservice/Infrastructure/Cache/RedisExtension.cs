#if redis
namespace Microservice.Infrastructure.Cache;
public static  class RedisExtension
{
    public static void AddRedisServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(option =>
        {
            option.Configuration = configuration.GetConnectionString("Redis");
            option.InstanceName = "Master";
        });
    }
}
#endif