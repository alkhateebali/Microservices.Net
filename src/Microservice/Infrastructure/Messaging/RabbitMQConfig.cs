using MassTransit;

namespace Microservice.Infrastructure.Messaging;

public  class RabbitMqSettings
{
    public string? Host { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}

public static class RabbitMQConfig
{
   
    public static void AddRabbitMqServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind RabbitMQ settings from configuration
        var rabbitMqSettings = new RabbitMqSettings();
        configuration.GetSection("RabbitMQ").Bind(rabbitMqSettings);

        // Register RabbitMQ settings
        services.AddSingleton(rabbitMqSettings);
        
        services.AddMassTransit(x =>
        {
            x.AddConsumer<Consumer>(cfg =>
            {
                cfg.UseMessageRetry(r
                    => r.Interval(3, TimeSpan.FromSeconds(5)));
            });


            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqSettings.Host ?? "rabbitmq://localhost",
                    h =>
                {
                    h.Username(rabbitMqSettings.UserName ?? string.Empty);
                    h.Password(rabbitMqSettings.Password ?? string.Empty);
                });

                cfg.ReceiveEndpoint("sample-queue",
                    e =>
                {
                    e.ConfigureConsumer<Consumer>(context);
                });
            });
        });
        services.AddTransient<IMessageProducer, Producer>();
        services.AddMassTransitHostedService();
    }
}
