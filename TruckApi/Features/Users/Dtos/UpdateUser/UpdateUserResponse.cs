namespace TruckApi.Features.Users.Dtos.UpdateUser;

/// <summary>Dados do usuário após atualização.</summary>
/// <param name="Id">Identificador único do usuário.</param>
/// <param name="FullName">Nome completo.</param>
/// <param name="Whatsapp">Número de WhatsApp no formato internacional.</param>
/// <param name="Role">Perfil de acesso.</param>
/// <param name="IsActive">Indica se o usuário está ativo.</param>
/// <param name="UpdatedAt">Data e hora da última atualização em UTC.</param>
public record UpdateUserResponse(
    string Id,
    string FullName,
    string Whatsapp,
    string Role,
    bool IsActive,
    DateTimeOffset UpdatedAt
);
