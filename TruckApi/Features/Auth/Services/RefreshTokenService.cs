using TruckApi.Features.Auth.Models;
using TruckApi.Infrastructure.Cache;

namespace TruckApi.Features.Auth.Services;

public class RefreshTokenService(
    ICacheService cache,
    IConfiguration config,
    IHttpContextAccessor httpContextAccessor
) : IRefreshTokenService
{
    private const int MaxDevices = 5;

    public async Task<string> GenerateAsync(string userId)
    {
        var token = TokenGenerator.New();
        var ttl = GetTtl();
        var data = BuildTokenData(userId);

        await cache.SetAsync(CacheKeys.Auth.RefreshToken(token), data, ttl);

        var userTokensKey = CacheKeys.Auth.UserTokens(userId);
        await cache.SortedSetAddAsync(userTokensKey, token, data.CreatedAt.ToUnixTimeMilliseconds());

        await EvictOldestIfOverLimitAsync(userTokensKey);

        return token;
    }

    public async Task<(string UserId, string NewToken)?> ValidateAndRotateAsync(string token)
    {
        var key = CacheKeys.Auth.RefreshToken(token);
        var data = await cache.GetAsync<RefreshTokenData>(key);

        if (data is null)
        {
            return null;
        }

        await cache.DeleteAsync(key);

        var userTokensKey = CacheKeys.Auth.UserTokens(data.UserId);
        await cache.SortedSetRemoveAsync(userTokensKey, token);

        var newToken = TokenGenerator.New();
        await cache.SetAsync(CacheKeys.Auth.RefreshToken(newToken), data, GetTtl());

        // preserve original creation time as score so oldest-session eviction stays correct
        await cache.SortedSetAddAsync(userTokensKey, newToken, data.CreatedAt.ToUnixTimeMilliseconds());

        return (data.UserId, newToken);
    }

    private async Task EvictOldestIfOverLimitAsync(string userTokensKey)
    {
        var count = await cache.SortedSetCountAsync(userTokensKey);

        if (count > MaxDevices)
        {
            var oldest = await cache.SortedSetRangeByRankAsync(userTokensKey, 0, 0);

            if (oldest.Length > 0)
            {
                await cache.DeleteAsync(CacheKeys.Auth.RefreshToken(oldest[0]));
                await cache.SortedSetRemoveAsync(userTokensKey, oldest[0]);
            }
        }
    }

    private RefreshTokenData BuildTokenData(string userId)
    {
        var request = httpContextAccessor.HttpContext?.Request;
        var connection = httpContextAccessor.HttpContext?.Connection;

        var ip = request?.Headers["X-Forwarded-For"].ToString() is { Length: > 0 } forwarded
            ? forwarded.Split(',')[0].Trim()
            : connection?.RemoteIpAddress?.ToString() ?? "unknown";

        var userAgent = request?.Headers["User-Agent"].ToString() ?? string.Empty;
        var origin = request?.Headers["Origin"].ToString() ?? string.Empty;
        var deviceName = ParseDeviceName(userAgent);

        return new RefreshTokenData(userId, ip, userAgent, origin, deviceName, DateTimeOffset.UtcNow);
    }

    private static string ParseDeviceName(string userAgent)
    {
        if (string.IsNullOrEmpty(userAgent))
        {
            return "Unknown";
        }

        string os;
        if (userAgent.Contains("iPhone")) { os = "iPhone"; }
        else if (userAgent.Contains("iPad")) { os = "iPad"; }
        else if (userAgent.Contains("Android")) { os = "Android"; }
        else if (userAgent.Contains("Windows")) { os = "Windows"; }
        else if (userAgent.Contains("Mac OS")) { os = "macOS"; }
        else if (userAgent.Contains("Linux")) { os = "Linux"; }
        else { os = "Unknown"; }

        string browser;
        if (userAgent.Contains("Edg/")) { browser = "Edge"; }
        else if (userAgent.Contains("Chrome/")) { browser = "Chrome"; }
        else if (userAgent.Contains("Firefox/")) { browser = "Firefox"; }
        else if (userAgent.Contains("Safari/")) { browser = "Safari"; }
        else { browser = "Unknown"; }

        return $"{os} · {browser}";
    }

    private TimeSpan GetTtl() =>
        TimeSpan.FromDays(config.GetValue("Jwt:RefreshTokenExpirationDays", 7));
}
