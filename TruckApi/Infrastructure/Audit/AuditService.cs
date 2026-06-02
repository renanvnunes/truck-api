using MongoDB.Driver;

namespace TruckApi.Infrastructure.Audit;

public class AuditService(IMongoCollection<AuditLog> collection) : IAuditService
{
    public Task LogAsync(AuditLog log) => collection.InsertOneAsync(log);
}
