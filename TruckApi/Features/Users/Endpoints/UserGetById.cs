using Carter;
using TruckApi.Features.Users.Dtos.GetUserById;
using TruckApi.Features.Users.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.Endpoints;

public class UserGetById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/users")
            .WithTags("Users")
            .MapGet(
                "/{id}",
                (GetUserByIdUseCase useCase, string id) =>
                    useCase
                        .ExecuteAsync(id)
                        .ToHttpResultAsync(user => new GetUserByIdResponse(
                            user.Id,
                            user.FullName,
                            user.Whatsapp,
                            user.Role.ToString(),
                            user.IsActive,
                            user.CreatedAt
                        ))
            )
            .WithSummary("Obter usuário por ID")
            .WithDescription("Retorna os dados de um usuário específico com base em seu ID.")
            .RequireAuth(UserRole.Admin, UserRole.CompanyManager, UserRole.CompanySupervisor);
    }
}
