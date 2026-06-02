namespace TruckApi.Features.Auth.Dtos.ForgotPassword;

/// <summary>Representa a solicitação para redefinição de senha.</summary>
/// <param name="Whatsapp">Número no formato internacional com 13 dígitos. Exemplo: 5511999999999.</param>
/// <param name="Code">Código de redefinição de senha.</param>
/// <param name="NewPassword">Nova senha do usuário.</param>
public record ConfirmNewPasswordRequest(string Whatsapp, string Code, string NewPassword);
