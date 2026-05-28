using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.Users.Dtos.GetUserById;
using TruckApi.Features.Users.Interface;

namespace TruckApi.Features.Users.Controllers;

public class UserGetById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/users")
            .WithTags("Users")
            .MapGet(
                "/{id}",
                async Task<Results<Ok<GetUserByIdResponse>, NotFound>> (
                    IUserRepository repository,
                    string id
                ) =>
                {
                    var user = await repository.GetByIdAsync(id);

                    if (user is null)
                    {
                        return TypedResults.NotFound();
                    }

                    return TypedResults.Ok(
                        new GetUserByIdResponse(
                            user.Id,
                            user.FullName,
                            user.Whatsapp,
                            user.Role.ToString(),
                            user.IsActive,
                            user.CreatedAt
                        )
                    );
                }
            )
            .WithSummary("Obter usuário por ID")
            .WithDescription("Retorna os dados de um usuário específico com base em seu ID.");
    }
}
