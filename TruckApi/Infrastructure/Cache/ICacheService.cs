namespace TruckApi.Infrastructure.Cache;

public interface ICacheService
{
    Task SetAsync<T>(string key, T value, TimeSpan ttl);
    Task<T?> GetAsync<T>(string key);
    Task DeleteAsync(string key);
}
