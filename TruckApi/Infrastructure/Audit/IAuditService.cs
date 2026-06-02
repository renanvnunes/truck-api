namespace TruckApi.Infrastructure.Audit;

public interface IAuditService
{
    Task LogAsync(AuditLog log);
}
