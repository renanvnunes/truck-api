namespace TruckApi.Features.Auth.Dtos.Otp;

/// <summary>Verificação do código OTP para login.</summary>
/// <param name="Whatsapp">Número no formato internacional com 13 dígitos. Exemplo: 5511999999999.</param>
/// <param name="Code">Código OTP recebido via WhatsApp.</param>
public record VerifyOtpRequest(string Whatsapp, string Code);
