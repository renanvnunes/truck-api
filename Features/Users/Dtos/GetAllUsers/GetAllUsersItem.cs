namespace TruckApi.Features.Users.Dtos.GetAllUsers;

/// <summary>Dados resumidos de um usuário na listagem.</summary>
/// <param name="Id">Identificador único do usuário.</param>
/// <param name="FullName">Nome completo.</param>
/// <param name="Whatsapp">Número de WhatsApp no formato internacional.</param>
/// <param name="Role">Perfil de acesso.</param>
/// <param name="IsActive">Indica se o usuário está ativo.</param>
/// <param name="CreatedAt">Data de criação em UTC.</param>
public record GetAllUsersItem(
    string Id,
    string FullName,
    string Whatsapp,
    string Role,
    bool IsActive,
    DateTimeOffset CreatedAt
);
