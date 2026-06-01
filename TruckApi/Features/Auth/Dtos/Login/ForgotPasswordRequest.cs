namespace TruckApi.Features.Auth.Dtos.Login;

/// <summary>Representa a solicitação para redefinição de senha.</summary>
/// <param name="Whatsapp">Número no formato internacional com 13 dígitos. Exemplo: 5511999999999.</param>
public record ForgotPasswordRequest(string Whatsapp);
