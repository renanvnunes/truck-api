using System.Text.Json;
using StackExchange.Redis;
using TruckApi.Shared;

namespace TruckApi.Features.Auth.Services;

public class SessionService(IConnectionMultiplexer redis) : ISessionService
{
    private static string Key(string userId) => $"session:{userId}";

    public async Task SaveAsync(string userId, UserSession session, TimeSpan ttl)
    {
        var db = redis.GetDatabase();
        var json = JsonSerializer.Serialize(session);
        await db.StringSetAsync(Key(userId), json, ttl);
    }

    public async Task<UserSession?> GetAsync(string userId)
    {
        var db = redis.GetDatabase();
        var value = await db.StringGetAsync(Key(userId));
        return value.IsNull ? null : JsonSerializer.Deserialize<UserSession>((string)value!);
    }

    public async Task DeleteAsync(string userId)
    {
        var db = redis.GetDatabase();
        await db.KeyDeleteAsync(Key(userId));
    }
}
