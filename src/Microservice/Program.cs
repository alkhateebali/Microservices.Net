using System.Reflection;
using System.Text;
using Asp.Versioning.ApiExplorer;
using FluentValidation;
using Microservice.Core.Config;
using Microservice.Core.EndPoints;
using Microservice.Core.Logging;
using Microservice.Core.Middlewares;
#if redis
using Microservice.Infrastructure.Cache;
#endif
using Microservice.Infrastructure.Health;
using Microservice.Infrastructure.Messaging;
using Microservice.Persistence.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microservice.Features.Items.Validation;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add Api versioning 
builder.Services.AddApiVersioningConfig(configuration);

// Add Validation 
builder.Services.AddValidatorsFromAssemblyContaining<CreateItemValidator>();


// Add service configurations 
builder.Services.AddServiceConfig(configuration);

//Metrics and Monitoring: Integrate tools like Prometheus and Grafana for monitoring.
builder.Services.AddMetrics();


// Authentication and Authorization

var jwtSettings = configuration.GetSection("Jwt");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? string.Empty))
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
    .AddPolicy("User", policy => policy.RequireRole("User")
        .RequireClaim("scope", "items_api"));;

builder.Services.AddDatabaseServices(configuration );

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())); 
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddLogging();
builder.Services.AddSingleton(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
builder.Logging.ClearProviders().AddConsole().AddDebug();

//Health checks -Database health
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database");
// implement Rate limiting

// Add RabbitMQ 
builder.Services.AddRabbitMqServices(configuration);

#if redis
// Add Redis cache 
builder.Services.AddRedisServices(configuration);
#endif

var app = builder.Build();

app.NewVersionedApi(ApiVersionsConfig.VersionSet?.ToString());
app.UseAuthentication();
app.UseAuthorization();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
    
}

app.RegisterEndpoints(new[] { Assembly.GetExecutingAssembly() });
app.MapHealthChecks("/health");

app.UseMiddleware<GlobalExceptionHandler>();

#if (messaging)
// Add RabbitMQ 
var consumer = app.Services.GetRequiredService<Consumer>();
consumer.StartConsuming();
#endif

app.Run();