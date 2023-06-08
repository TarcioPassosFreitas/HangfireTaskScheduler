using HangfireTaskScheduler.Core.Aggregate.UserAggregate;
using HangfireTaskScheduler.Infraestructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace HangfireTaskScheduler.Infraestructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}