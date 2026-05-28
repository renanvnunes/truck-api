using System.Diagnostics;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.Users.Errors;
using TruckApi.Features.Users.UseCases;
using TruckApi.Infrastructure.Database.Entities;
using TruckApi.Shared;

namespace TruckApi.Features.Users.Controllers;

public class UserDelete : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/users")
            .WithTags("Users")
            .MapDelete(
                "/{id}",
                async Task<Results<NoContent, NotFound<ErrorResponse>>> (
                    string id,
                    DeleteUserUseCase useCase
                ) =>
                {
                    return await useCase.ExecuteAsync(id) switch
                    {
                        Result<User>.Ok => TypedResults.NoContent(),
                        Result<User>.Fail { Error: var error } => TypedResults.NotFound(
                            new ErrorResponse(error.Code, error.Message)
                        ),
                        _ => throw new UnreachableException(),
                    };
                }
            )
            .WithSummary("Remover usuário")
            .WithDescription("Remove permanentemente um usuário do sistema.");
    }
}
