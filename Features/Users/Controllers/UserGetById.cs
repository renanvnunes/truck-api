using System.Diagnostics;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.Users.Dtos.GetUserById;
using TruckApi.Features.Users.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.Controllers;

public class UserGetById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/users")
            .WithTags("Users")
            .MapGet(
                "/{id}",
                async Task<Results<Ok<GetUserByIdResponse>, NotFound<ErrorResponse>>> (
                    GetUserByIdUseCase useCase,
                    string id
                ) =>
                {
                    return await useCase.ExecuteAsync(id) switch
                    {
                        Result<User>.Ok { Value: var user } => TypedResults.Ok(
                            new GetUserByIdResponse(
                                user.Id,
                                user.FullName,
                                user.Whatsapp,
                                user.Role.ToString(),
                                user.IsActive,
                                user.CreatedAt
                            )
                        ),
                        Result<User>.Fail { Error: var error } => TypedResults.NotFound(
                            new ErrorResponse(error.Code, error.Message)
                        ),
                        _ => throw new UnreachableException(),
                    };
                }
            )
            .WithSummary("Obter usuário por ID")
            .WithDescription("Retorna os dados de um usuário específico com base em seu ID.");
    }
}
