using TruckApi.Infrastructure.Cache;

namespace TruckApi.Features.Auth.Services;

public class RefreshTokenService(ICacheService cache) : IRefreshTokenService
{
    private static readonly TimeSpan Ttl = TimeSpan.FromDays(30);
    private const string KeyPrefix = "refresh_token:";

    public async Task<string> GenerateAsync(string userId)
    {
        var token = TokenGenerator.New();
        await cache.SetAsync($"{KeyPrefix}{token}", userId, Ttl);
        return token;
    }

    public async Task<(string UserId, string NewToken)?> ValidateAndRotateAsync(string token)
    {
        var key = $"{KeyPrefix}{token}";
        var userId = await cache.GetAsync<string>(key);
        if (userId is null)
            return null;

        await cache.DeleteAsync(key);

        var newToken = TokenGenerator.New();
        await cache.SetAsync($"{KeyPrefix}{newToken}", userId, Ttl);

        return (userId, newToken);
    }
}
