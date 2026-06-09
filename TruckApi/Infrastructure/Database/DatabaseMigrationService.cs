using Microsoft.EntityFrameworkCore;

namespace TruckApi.Infrastructure.Database;

public class DatabaseMigrationService(
    IServiceScopeFactory scopeFactory,
    ILogger<DatabaseMigrationService> logger
) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var pending = (await db.Database.GetPendingMigrationsAsync(cancellationToken)).ToList();

        if (pending.Count == 0)
        {
            logger.LogInformation("Database is up to date. No pending migrations.");
            return;
        }

        logger.LogWarning(
            "{Count} pending migration(s) detected. Run manually: dotnet ef database update. Pending: {Migrations}",
            pending.Count,
            string.Join(", ", pending)
        );
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
