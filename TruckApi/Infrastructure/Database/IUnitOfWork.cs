namespace TruckApi.Infrastructure.Database;

public interface IUnitOfWork
{
    Task CommitAsync();
}
