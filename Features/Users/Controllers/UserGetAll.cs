using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.Users.Dtos.GetAllUsers;
using TruckApi.Features.Users.Interface;

namespace TruckApi.Features.Users.Controllers;

public class UserGetAll : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/users")
            .WithTags("Users")
            .MapGet(
                "/",
                async Task<Ok<GetAllUsersResponse[]>> (IUserRepository repository) =>
                {
                    var users = await repository.GetAllAsync();

                    var response = users
                        .Select(u => new GetAllUsersResponse(
                            u.Id,
                            u.FullName,
                            u.Whatsapp,
                            u.Role.ToString(),
                            u.IsActive,
                            u.CreatedAt
                        ))
                        .ToArray();

                    return TypedResults.Ok(response);
                }
            )
            .WithSummary("Listar usuários")
            .WithDescription("Retorna todos os usuários cadastrados no sistema.");
    }
}
