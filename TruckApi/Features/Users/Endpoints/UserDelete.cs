using Carter;
using TruckApi.Features.Users.UseCases;

namespace TruckApi.Features.Users.Endpoints;

public class UserDelete : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/users")
            .WithTags("Users")
            .MapDelete(
                "/{id}",
                (string id, DeleteUserUseCase useCase) =>
                    useCase.ExecuteAsync(id).ToNoContentAsync()
            )
            .WithSummary("Remover usuário")
            .WithDescription("Remove permanentemente um usuário do sistema.");
    }
}
