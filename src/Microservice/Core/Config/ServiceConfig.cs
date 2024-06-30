namespace Microservice.Core.Config;

public class ServiceSettings
{
    public const string Service = "ServiceOptions";
    public bool FeatureToggle { get; set; }
    public string? Environment { get; set; }
    
}
public static class ServiceConfig
{
   
    public static void AddServiceConfig(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind RabbitMQ settings from configuration
        var serviceConfig = new ServiceSettings();
        configuration.GetSection(ServiceSettings.Service).Bind(serviceConfig);
        
    }
}