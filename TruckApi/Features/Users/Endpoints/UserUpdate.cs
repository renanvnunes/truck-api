using Carter;
using TruckApi.Features.Users.Dtos.UpdateUser;
using TruckApi.Features.Users.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.Endpoints;

public class UserUpdate : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/users")
            .WithTags("Users")
            .MapPatch(
                "/{id}",
                (string id, UpdateUserRequest request, UpdateUserUseCase useCase) =>
                    useCase
                        .ExecuteAsync(id, request)
                        .ToHttpResultAsync(user => new UpdateUserResponse(
                            user.Id,
                            user.FullName,
                            user.Whatsapp,
                            user.Role.ToString(),
                            user.IsActive,
                            user.UpdatedAt
                        ))
            )
            .WithSummary("Atualizar usuário")
            .WithDescription(
                "Atualiza parcialmente os dados de um usuário. Apenas os campos enviados serão alterados."
            )
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<UpdateUserRequest>>()
            .RequireAuth(UserRole.Admin, UserRole.CompanyManager);
    }
}
