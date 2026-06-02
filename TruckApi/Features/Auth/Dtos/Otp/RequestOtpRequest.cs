namespace TruckApi.Features.Auth.Dtos.Otp;

/// <summary>Solicitação de código OTP para login.</summary>
/// <param name="Whatsapp">Número no formato internacional com 13 dígitos. Exemplo: 5511999999999.</param>
public record RequestOtpRequest(string Whatsapp);
