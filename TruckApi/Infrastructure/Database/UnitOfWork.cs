namespace TruckApi.Infrastructure.Database;

public class UnitOfWork(AppDbContext db) : IUnitOfWork
{
    public Task CommitAsync() => db.SaveChangesAsync();
}
