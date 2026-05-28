using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.Dtos.CreateUser;

/// <summary>Dados para criação de um novo usuário.</summary>
/// <param name="FullName">Nome completo do usuário.</param>
/// <param name="Whatsapp">Número no formato internacional com 13 dígitos. Exemplo: 5511999999999.</param>
/// <param name="Password">Senha de acesso. Mínimo de 6 caracteres. Opcional.</param>
/// <param name="Document">CPF ou outro documento de identificação. Opcional.</param>
/// <param name="Age">Idade do usuário. Deve ser entre 16 e 80 anos. Opcional.</param>
/// <param name="Role">Perfil de acesso. Padrão: CompanyOperator.</param>
/// <param name="CompanyId">ID da empresa à qual o usuário pertence. Opcional.</param>
/// <param name="Timezone">Fuso horário no formato IANA. Padrão: America/Sao_Paulo.</param>
public record CreateUserRequest(
    string FullName,
    string Whatsapp,
    string? Password = null,
    string? Document = null,
    int? Age = null,
    UserRole Role = UserRole.CompanyOperator,
    string? CompanyId = null,
    string Timezone = "America/Sao_Paulo"
);
