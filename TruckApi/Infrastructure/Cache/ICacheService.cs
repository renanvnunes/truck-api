namespace TruckApi.Infrastructure.Cache;

public interface ICacheService
{
    Task SetAsync<T>(string key, T value, TimeSpan ttl);
    Task<T?> GetAsync<T>(string key);
    Task<bool> ExistsAsync(string key);
    Task DeleteAsync(string key);

    Task SortedSetAddAsync(string key, string member, double score);
    Task SortedSetRemoveAsync(string key, string member);
    Task<long> SortedSetCountAsync(string key);
    Task<string[]> SortedSetRangeByRankAsync(string key, long start, long stop);
}
