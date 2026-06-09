using TruckApi.Features.Auth.Dtos.ForgotPassword;
using TruckApi.Features.Auth.Errors;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Infrastructure.Audit;
using TruckApi.Infrastructure.Cache;

namespace TruckApi.Features.Auth.UseCases;

public class ConfirmNewPasswordUseCase(
    IUserRepository userRepository,
    ICacheService cacheService,
    IAuditService auditService,
    IUnitOfWork unitOfWork
)
{
    public async Task<Result<ConfirmNewPasswordResponse>> ExecuteAsync(ConfirmNewPasswordRequest request)
    {
        var cacheKey = CacheKeys.Auth.Forgot.Code(request.Whatsapp);
        var cached = await cacheService.GetAsync<PasswordResetCache>(cacheKey);

        if (cached is null || cached.Code != request.Code)
        {
            return Result<ConfirmNewPasswordResponse>.Failure(AuthErrors.InvalidVerificationCode);
        }

        await cacheService.DeleteAsync(cacheKey);

        var passwordHash = PasswordHash.Hash(request.NewPassword);
        await userRepository.UpdatePasswordAsync(cached.UserId, passwordHash);
        await unitOfWork.CommitAsync();

        _ = auditService.LogAsync(
            new AuditLog
            {
                Event = AuditEvent.UserPasswordChanged,
                UserId = cached.UserId,
                Metadata = new() { ["whatsapp"] = request.Whatsapp },
            }
        );

        return Result<ConfirmNewPasswordResponse>.Success(
            new ConfirmNewPasswordResponse("Senha atualizada com sucesso!")
        );
    }
}
