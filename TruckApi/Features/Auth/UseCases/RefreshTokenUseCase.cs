using TruckApi.Features.Auth.Dtos.Refresh;
using TruckApi.Features.Auth.Errors;
using TruckApi.Features.Auth.Services;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Infrastructure.Cache;

namespace TruckApi.Features.Auth.UseCases;

public class RefreshTokenUseCase(
    IRefreshTokenService refreshTokenService,
    IUserRepository userRepository,
    ITokenService tokenService,
    ICacheService cache
)
{
    public async Task<Result<RefreshTokenResponse>> ExecuteAsync(RefreshTokenRequest request)
    {
        var rotated = await refreshTokenService.ValidateAndRotateAsync(request.RefreshToken);
        if (rotated is null)
            return Result<RefreshTokenResponse>.Failure(AuthErrors.InvalidRefreshToken);

        var (userId, newRefreshToken) = rotated.Value;

        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
            return Result<RefreshTokenResponse>.Failure(AuthErrors.UserNotFound);

        if (!user.IsActive)
            return Result<RefreshTokenResponse>.Failure(AuthErrors.UserInactive);

        var accessToken = tokenService.Generate(user, out var expiration);

        await cache.SetAsync(
            CacheKeys.Auth.Session(user.Id),
            new UserSession(user.Id, user.FullName, user.Whatsapp, user.Role.ToString(), user.CompanyId, user.IsActive),
            expiration
        );

        return Result<RefreshTokenResponse>.Success(new RefreshTokenResponse(accessToken, newRefreshToken));
    }
}
