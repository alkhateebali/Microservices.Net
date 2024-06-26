using Microservice.Features.Items.Domain;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Persistence.Database;
public  class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Item> Registrations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}


