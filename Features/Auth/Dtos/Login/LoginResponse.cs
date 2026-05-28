
namespace TruckApi.Features.Auth.Dtos.Login;

/// <summary>Resposta do login com token e dados do usuário autenticado.</summary>
/// <param name="Token">JWT para uso nas próximas requisições.</param>
/// <param name="User">Dados do usuário autenticado.</param>
public record LoginResponse(string Token, UserSession User);
