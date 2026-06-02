using TruckApi.Features.Auth.Dtos.Otp;
using TruckApi.Features.Auth.Errors;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Infrastructure.Cache;

namespace TruckApi.Features.Auth.UseCases;

public class RequestOtpUseCase(IUserRepository userRepository, ICacheService cacheService)
{
    private const string CodeKeyPrefix = "otp-login-code:";
    private const string CooldownKeyPrefix = "otp-login-cooldown:";
    private const int CooldownSeconds = 30;
    private const int ExpiresInSeconds = 300;

    public async Task<Result<RequestOtpResponse>> ExecuteAsync(RequestOtpRequest request)
    {
        var user = await userRepository.GetByWhatsappAsync(request.Whatsapp);

        if (user is null)
            return Result<RequestOtpResponse>.Failure(AuthErrors.UserNotFound);

        if (!user.IsActive)
            return Result<RequestOtpResponse>.Failure(AuthErrors.UserInactive);

        var cooldownKey = $"{CooldownKeyPrefix}{request.Whatsapp}";

        if (await cacheService.ExistsAsync(cooldownKey))
            return Result<RequestOtpResponse>.Failure(AuthErrors.OtpCooldown);

        var code = NumberGenerator.New(6);

        await Task.WhenAll(
            cacheService.SetAsync(
                $"{CodeKeyPrefix}{request.Whatsapp}",
                code,
                TimeSpan.FromSeconds(ExpiresInSeconds)
            ),
            cacheService.SetAsync(
                cooldownKey,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                TimeSpan.FromSeconds(CooldownSeconds)
            )
        );

        return Result<RequestOtpResponse>.Success(
            new RequestOtpResponse(request.Whatsapp, ExpiresInSeconds)
        );
    }
}
