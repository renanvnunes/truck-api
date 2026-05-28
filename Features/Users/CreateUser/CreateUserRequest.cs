using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.CreateUser;

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
