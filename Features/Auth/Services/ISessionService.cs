using TruckApi.Shared;

namespace TruckApi.Features.Auth.Services;

public interface ISessionService
{
    Task SaveAsync(string userId, UserSession session, TimeSpan ttl);
    Task<UserSession?> GetAsync(string userId);
    Task DeleteAsync(string userId);
}
