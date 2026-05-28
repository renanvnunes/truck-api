namespace TruckApi.Features.Users.Dtos.GetUserById;

/// <summary>Dados de um usuário específico.</summary>
/// <param name="Id">Identificador único do usuário.</param>
/// <param name="FullName">Nome completo.</param>
/// <param name="Whatsapp">Número de WhatsApp no formato internacional.</param>
/// <param name="Role">Perfil de acesso.</param>
/// <param name="IsActive">Indica se o usuário está ativo.</param>
/// <param name="CreatedAt">Data de criação em UTC.</param>
public record GetUserByIdResponse(
    string Id,
    string FullName,
    string Whatsapp,
    string Role,
    bool IsActive,
    DateTimeOffset CreatedAt
);
