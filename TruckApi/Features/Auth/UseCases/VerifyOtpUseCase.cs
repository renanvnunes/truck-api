using TruckApi.Features.Auth.Dtos.Login;
using TruckApi.Features.Auth.Dtos.Otp;
using TruckApi.Features.Auth.Errors;
using TruckApi.Features.Auth.Services;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Infrastructure.Cache;

namespace TruckApi.Features.Auth.UseCases;

public class VerifyOtpUseCase(
    IUserRepository userRepository,
    ITokenService tokenService,
    IRefreshTokenService refreshTokenService,
    ICacheService cacheService
)
{
    public async Task<Result<LoginResponse>> ExecuteAsync(VerifyOtpRequest request)
    {
        var cacheKey = CacheKeys.Auth.Otp.Code(request.Whatsapp);
        var cachedCode = await cacheService.GetAsync<string>(cacheKey);

        if (cachedCode is null || cachedCode != request.Code)
        {
            return Result<LoginResponse>.Failure(AuthErrors.InvalidVerificationCode);
        }

        var user = await userRepository.GetByWhatsappAsync(request.Whatsapp);

        if (user is null)
        {
            return Result<LoginResponse>.Failure(AuthErrors.UserNotFound);
        }

        if (!user.IsActive)
        {
            return Result<LoginResponse>.Failure(AuthErrors.UserInactive);
        }

        await cacheService.DeleteAsync(cacheKey);

        var token = tokenService.Generate(user, out var expiration);
        var refreshToken = await refreshTokenService.GenerateAsync(user.Id);

        var session = new UserSession(
            user.Id,
            user.FullName,
            user.Whatsapp,
            user.Role.ToString(),
            user.CompanyId,
            user.IsActive
        );

        await cacheService.SetAsync(CacheKeys.Auth.Session(user.Id), session, expiration);

        return Result<LoginResponse>.Success(new LoginResponse(token, refreshToken, session));
    }
}
