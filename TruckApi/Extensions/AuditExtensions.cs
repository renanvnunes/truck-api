using MongoDB.Driver;
using TruckApi.Infrastructure.Audit;

namespace TruckApi.Extensions;

public static class AuditExtensions
{
    public static IServiceCollection AddAudit(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var connectionString = config.GetConnectionString("MongoDB")!;
        var databaseName = config.GetValue<string>("MongoDB:Database")!;

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        var collection = database.GetCollection<AuditLog>("audit_logs");

        EnsureIndexes(collection);

        services.AddSingleton(collection);
        services.AddSingleton<IAuditService, AuditService>();

        return services;
    }

    private static void EnsureIndexes(IMongoCollection<AuditLog> collection)
    {
        var indexes = new[]
        {
            new CreateIndexModel<AuditLog>(Builders<AuditLog>.IndexKeys.Ascending(x => x.Event)),
            new CreateIndexModel<AuditLog>(Builders<AuditLog>.IndexKeys.Ascending(x => x.UserId)),
            new CreateIndexModel<AuditLog>(
                Builders<AuditLog>.IndexKeys.Descending(x => x.OccurredAt)
            ),
            // index composto para consultas do tipo: "todos os eventos de um usuário por data"
            new CreateIndexModel<AuditLog>(
                Builders<AuditLog>.IndexKeys.Ascending(x => x.UserId).Descending(x => x.OccurredAt)
            ),
        };

        collection.Indexes.CreateMany(indexes);
    }
}
