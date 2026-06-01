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

    public async Task DeleteAsync(string key)
    {
        var db = redis.GetDatabase();
        await db.KeyDeleteAsync(key);
    }
}
