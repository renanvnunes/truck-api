using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TruckApi.Infrastructure.Cache;
using TruckApi.Infrastructure.Database;

namespace TruckApi.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Postgres"))
        );

        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(config.GetConnectionString("Redis")!)
        );

        services.AddSingleton<ICacheService, CacheService>();
        services.AddHostedService<DatabaseMigrationService>();

        return services;
    }
}
