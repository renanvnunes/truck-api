using TruckApi.Features.Auth.Dtos.ForgotPassword;
using TruckApi.Features.Auth.Dtos.Login;
using TruckApi.Features.Auth.Errors;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Infrastructure.Cache;

namespace TruckApi.Features.Auth.UseCases;

public class ForgotPasswordUseCase(IUserRepository userRepository, ICacheService cacheService)
{
    private const int CooldownSeconds = 30;
    private const int ExpiresInSeconds = 300;

    public async Task<Result<ForgotPasswordResponse>> ExecuteAsync(ForgotPasswordRequest request)
    {
        var user = await userRepository.GetByWhatsappAsync(request.Whatsapp);

        if (user is null)
        {
            return Result<ForgotPasswordResponse>.Failure(AuthErrors.UserNotFound);
        }

        if (!user.IsActive)
        {
            return Result<ForgotPasswordResponse>.Failure(AuthErrors.UserInactive);
        }

        if (await cacheService.ExistsAsync(CacheKeys.Auth.Forgot.Cooldown(request.Whatsapp)))
        {
            return Result<ForgotPasswordResponse>.Failure(AuthErrors.ForgotPasswordCooldown);
        }

        var resetCode = NumberGenerator.New(6);

        await Task.WhenAll(
            cacheService.SetAsync(
                CacheKeys.Auth.Forgot.Code(request.Whatsapp),
                new PasswordResetCache(user.Id, resetCode),
                TimeSpan.FromSeconds(ExpiresInSeconds)
            ),
            cacheService.SetAsync(
                CacheKeys.Auth.Forgot.Cooldown(request.Whatsapp),
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                TimeSpan.FromSeconds(CooldownSeconds)
            )
        );

        return Result<ForgotPasswordResponse>.Success(
            new ForgotPasswordResponse(request.Whatsapp, ExpiresInSeconds)
        );
    }
}
