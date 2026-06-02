namespace TruckApi.Features.Auth.Dtos.ForgotPassword;

public record PasswordResetCache(string UserId, string Code);
