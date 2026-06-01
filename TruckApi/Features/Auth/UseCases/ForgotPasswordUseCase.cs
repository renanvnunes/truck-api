using TruckApi.Features.Auth.Dtos.Login;
using TruckApi.Features.Auth.Errors;
using TruckApi.Features.Users.Interfaces;

namespace TruckApi.Features.Auth.UseCases;

public class ForgotPasswordUseCase(IUserRepository userRepository)
{
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

        const int expiresInSeconds = 300;
        var maskedPhone = MaskPhone(request.Whatsapp);

        return Result<ForgotPasswordResponse>.Success(
            new ForgotPasswordResponse(maskedPhone, expiresInSeconds)
        );
    }

    private static string MaskPhone(string phone)
    {
        if (phone.Length <= 4)
            return phone;

        var visible = phone[^4..];
        var masked = new string('*', phone.Length - 4);
        return masked + visible;
    }
}
