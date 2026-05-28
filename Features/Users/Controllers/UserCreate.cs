using System.Diagnostics;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.Users.Dtos.CreateUser;
using TruckApi.Features.Users.UseCases;
using TruckApi.Infrastructure.Database.Entities;
using TruckApi.Shared;

namespace TruckApi.Features.Users.Controllers;

public class UserCreate : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/users")
            .WithTags("Users")
            .MapPost(
                "/",
                async Task<Results<Created<CreateUserResponse>, Conflict<ErrorResponse>>> (
                    CreateUserRequest request,
                    CreateUserUseCase useCase
                ) =>
                {
                    // nunca expõe o hash da senha na resposta
                    return await useCase.ExecuteAsync(request) switch
                    {
                        Result<User>.Ok { Value: var user } => TypedResults.Created(
                            $"/users/{user.Id}",
                            new CreateUserResponse(
                                user.Id,
                                user.FullName,
                                user.Whatsapp,
                                user.Role.ToString(),
                                user.IsActive,
                                user.CreatedAt
                            )
                        ),
                        Result<User>.Fail { Error: var error } => TypedResults.Conflict(
                            new ErrorResponse(error.Code, error.Message)
                        ),
                        _ => throw new UnreachableException(),
                    };
                }
            )
            .WithSummary("Criar usuário")
            .WithDescription(
                "Cria um novo usuário no sistema. O whatsapp deve estar no formato internacional com 13 dígitos (ex: 5511999999999)."
            )
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<CreateUserRequest>>();
    }
}
