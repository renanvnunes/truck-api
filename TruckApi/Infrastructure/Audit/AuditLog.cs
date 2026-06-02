using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TruckApi.Infrastructure.Audit;

public class AuditLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRepresentation(BsonType.String)]
    public AuditEvent Event { get; set; }

    public string? UserId { get; set; }
    public string? PerformedBy { get; set; }
    public string? Ip { get; set; }
    public Dictionary<string, object?>? Metadata { get; set; }
    public DateTimeOffset OccurredAt { get; set; } = DateTimeOffset.UtcNow;
}
