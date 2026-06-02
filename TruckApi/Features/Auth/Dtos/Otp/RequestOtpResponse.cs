namespace TruckApi.Features.Auth.Dtos.Otp;

/// <summary>Resposta da solicitação de código OTP.</summary>
public record RequestOtpResponse(string Phone, int ExpiresInSeconds);
