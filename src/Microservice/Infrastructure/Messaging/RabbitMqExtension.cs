namespace Microservice.Infrastructure.Messaging;

public  class RabbitMqSettings
{
    public string? Host { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}
#if (messaging)
public static class RabbitMqExtension
{
   
    public static void AddRabbitMqServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind RabbitMQ settings from configuration
        var rabbitMqSettings = new RabbitMqSettings();
        configuration.GetSection("RabbitMQ").Bind(rabbitMqSettings);

        // Register RabbitMQ settings
        services.AddSingleton(rabbitMqSettings);

        services.AddSingleton<Publisher>();
        services.AddSingleton<Consumer>();
    }
}
#endif
