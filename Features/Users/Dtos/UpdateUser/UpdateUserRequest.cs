using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.Dtos.UpdateUser;

/// <summary>Dados para atualização de um usuário. Apenas os campos enviados serão alterados.</summary>
/// <param name="FullName">Nome completo do usuário.</param>
/// <param name="Whatsapp">Número no formato internacional com 13 dígitos. Exemplo: 5511999999999.</param>
/// <param name="Password">Nova senha de acesso. Mínimo de 6 caracteres.</param>
/// <param name="Document">CPF ou outro documento de identificação.</param>
/// <param name="Age">Idade do usuário. Deve ser entre 18 e 90 anos.</param>
/// <param name="Role">Perfil de acesso.</param>
/// <param name="Timezone">Fuso horário no formato IANA.</param>
/// <param name="IsActive">Indica se o usuário está ativo.</param>
public record UpdateUserRequest(
    string? FullName = null,
    string? Whatsapp = null,
    string? Password = null,
    string? Document = null,
    int? Age = null,
    UserRole? Role = null,
    string? Timezone = null,
    bool? IsActive = null
);
