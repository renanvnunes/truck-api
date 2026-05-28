using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.Users.Dtos.GetAllUsers;
using TruckApi.Features.Users.UseCases;

namespace TruckApi.Features.Users.Controllers;

public class UserGetAll : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/users")
            .WithTags("Users")
            .MapGet(
                "/",
                async Task<Ok<GetAllUsersResponse>> (
                    GetAllUsersUseCase useCase,
                    string? cursor = null,
                    int limit = 20
                ) =>
                {
                    var (users, nextCursor) = await useCase.ExecuteAsync(cursor, limit);

                    var response = new GetAllUsersResponse(
                        users
                            .Select(u => new GetAllUsersItem(
                                u.Id,
                                u.FullName,
                                u.Whatsapp,
                                u.Role.ToString(),
                                u.IsActive,
                                u.CreatedAt
                            ))
                            .ToArray(),
                        nextCursor
                    );

                    return TypedResults.Ok(response);
                }
            )
            .WithSummary("Listar usuários")
            .WithDescription(
                "Retorna usuários paginados com cursor. Use o campo `nextCursor` da resposta como parâmetro `cursor` na próxima requisição."
            );
    }
}
