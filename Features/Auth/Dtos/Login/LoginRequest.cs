namespace TruckApi.Features.Auth.Dtos.Login;

/// <summary>Credenciais para autenticação.</summary>
/// <param name="Whatsapp">Número no formato internacional com 13 dígitos. Exemplo: 5511999999999.</param>
/// <param name="Password">Senha de acesso.</param>
public record LoginRequest(string Whatsapp, string Password);
