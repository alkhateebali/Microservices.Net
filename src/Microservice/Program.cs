using System.Reflection;
using System.Text;
using Microservice.Core.EndPoints;
using Microservice.Core.Logging;
using Microservice.Infrastructure.Health;
using Microservice.Infrastructure.Messaging;
using Microservice.Persistence.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;



var builder = WebApplication.CreateBuilder(args);

//Health checks
// Database health
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Database");

//Metrics and Monitoring: Integrate tools like Prometheus and Grafana for monitoring.
builder.Services.AddMetrics();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(x => x.FullName?.Replace("+", ".", StringComparison.Ordinal));
});
// Authentication and Authorization
var configuration = builder.Configuration;
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
    .AddPolicy("User", policy => policy.RequireRole("User"));

builder.Services.AddDatabaseServices();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())); 
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddLogging();
builder.Services.AddSingleton(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
builder.Logging.ClearProviders().AddConsole().AddDebug();

// Add RabbitMQ 
builder.Services.AddRabbitMqServices(configuration);

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.RegisterEndpoints(new[] { Assembly.GetExecutingAssembly() });
app.MapHealthChecks("/health");
// var consumer = app.ApplicationServices.GetRequiredService<Consumer>();
// consumer.StartConsuming();
app.Run();