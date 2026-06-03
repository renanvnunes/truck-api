using System.Text.Json;
using StackExchange.Redis;

namespace TruckApi.Infrastructure.Cache;

public class CacheService(IConnectionMultiplexer redis) : ICacheService
{
    public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
    {
        var db = redis.GetDatabase();
        var json = JsonSerializer.Serialize(value);
        await db.StringSetAsync(key, json, ttl);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var db = redis.GetDatabase();
        var value = await db.StringGetAsync(key);
        return value.IsNull ? default : JsonSerializer.Deserialize<T>((string)value!);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        var db = redis.GetDatabase();
        return await db.KeyExistsAsync(key);
    }

    public async Task DeleteAsync(string key)
    {
        var db = redis.GetDatabase();
        await db.KeyDeleteAsync(key);
    }

    public async Task SortedSetAddAsync(string key, string member, double score)
    {
        var db = redis.GetDatabase();
        await db.SortedSetAddAsync(key, member, score);
    }

    public async Task SortedSetRemoveAsync(string key, string member)
    {
        var db = redis.GetDatabase();
        await db.SortedSetRemoveAsync(key, member);
    }

    public async Task<long> SortedSetCountAsync(string key)
    {
        var db = redis.GetDatabase();
        return await db.SortedSetLengthAsync(key);
    }

    public async Task<string[]> SortedSetRangeByRankAsync(string key, long start, long stop)
    {
        var db = redis.GetDatabase();
        var values = await db.SortedSetRangeByRankAsync(key, start, stop);
        return values.Select(v => (string)v!).ToArray();
    }
}
