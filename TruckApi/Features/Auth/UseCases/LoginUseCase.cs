using TruckApi.Features.Auth.Dtos.Login;
using TruckApi.Features.Auth.Errors;
using TruckApi.Features.Auth.Services;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Infrastructure.Cache;

namespace TruckApi.Features.Auth.UseCases;

public class LoginUseCase(
    IUserRepository userRepository,
    ITokenService tokenService,
    ICacheService cache
)
{
    public async Task<Result<LoginResponse>> ExecuteAsync(LoginRequest request)
    {
        var user = await userRepository.GetByWhatsappAsync(request.Whatsapp);

        if (user is null || !PasswordHash.Verify(request.Password, user.Password ?? string.Empty))
        {
            return Result<LoginResponse>.Failure(AuthErrors.InvalidCredentials);
        }

        if (!user.IsActive)
        {
            return Result<LoginResponse>.Failure(AuthErrors.UserInactive);
        }

        var token = tokenService.Generate(user, out var expiration);

        var session = new UserSession(
            user.Id,
            user.FullName,
            user.Whatsapp,
            user.Role.ToString(),
            user.CompanyId,
            user.IsActive
        );

        await cache.SetAsync($"session:{user.Id}", session, expiration);

        return Result<LoginResponse>.Success(new LoginResponse(token, session));
    }
}
