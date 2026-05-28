namespace TruckApi.Features.Users.Dtos.CreateUser;

/// <summary>Dados do usuário criado.</summary>
/// <param name="Id">Identificador único gerado automaticamente.</param>
/// <param name="FullName">Nome completo do usuário.</param>
/// <param name="Whatsapp">Número de WhatsApp no formato internacional.</param>
/// <param name="Role">Perfil de acesso atribuído.</param>
/// <param name="IsActive">Indica se o usuário está ativo. Padrão: false até ativação manual.</param>
/// <param name="CreatedAt">Data e hora de criação em UTC.</param>
public record CreateUserResponse(
    string Id,
    string FullName,
    string Whatsapp,
    string Role,
    bool IsActive,
    DateTimeOffset CreatedAt
);
