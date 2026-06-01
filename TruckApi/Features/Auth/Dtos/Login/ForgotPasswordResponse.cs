namespace TruckApi.Features.Auth.Dtos.Login;

/// <summary>Resposta da solicitação de redefinição de senha.</summary>
public record ForgotPasswordResponse(string MaskedPhone, int ExpiresInSeconds);
