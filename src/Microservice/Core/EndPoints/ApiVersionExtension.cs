using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Microsoft.OpenApi.Models;

namespace Microservice.Core.EndPoints;


public static class ApiVersionExtension
{
    
    public static void AddApiVersioningConfig(this IServiceCollection services, IConfiguration configuration)
    {
        
       
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader("api-version"),
                new HeaderApiVersionReader("X-Version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        
        services.AddSwaggerGen(options =>
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            options.CustomSchemaIds(x => x.FullName?.Replace("+", ".", StringComparison.Ordinal));

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo()
                {
                    Title = $"Service API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString()
                });
            }
        });

    }
    
}
