using Microservice.Persistence.Database;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microservice.Infrastructure.Health;

public class DatabaseHealthCheck(AppDbContext context) : IHealthCheck
{
    private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            // Check database connectivity
            var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
            return canConnect ? HealthCheckResult.Healthy("Database is reachable") : HealthCheckResult.Unhealthy("Database is not reachable");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"An error occurred: {ex.Message}");
        }
    }

    
}