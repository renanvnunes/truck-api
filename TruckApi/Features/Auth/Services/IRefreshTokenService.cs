namespace TruckApi.Features.Auth.Services;

public interface IRefreshTokenService
{
    Task<string> GenerateAsync(string userId);
    Task<(string UserId, string NewToken)?> ValidateAndRotateAsync(string token);
}
