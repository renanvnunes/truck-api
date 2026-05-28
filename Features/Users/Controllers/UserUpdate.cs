using System.Diagnostics;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.Users.Dtos.UpdateUser;
using TruckApi.Features.Users.Errors;
using TruckApi.Features.Users.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.Controllers;

public class UserUpdate : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/users")
            .WithTags("Users")
            .MapPatch(
                "/{id}",
                async Task<
                    Results<
                        Ok<UpdateUserResponse>,
                        NotFound<ErrorResponse>,
                        Conflict<ErrorResponse>
                    >
                > (string id, UpdateUserRequest request, UpdateUserUseCase useCase) =>
                {
                    return await useCase.ExecuteAsync(id, request) switch
                    {
                        Result<User>.Ok { Value: var user } => TypedResults.Ok(
                            new UpdateUserResponse(
                                user.Id,
                                user.FullName,
                                user.Whatsapp,
                                user.Role.ToString(),
                                user.IsActive,
                                user.UpdatedAt
                            )
                        ),
                        Result<User>.Fail { Error: var error } when error == UserErrors.NotFound =>
                            TypedResults.NotFound(new ErrorResponse(error.Code, error.Message)),
                        Result<User>.Fail { Error: var error } => TypedResults.Conflict(
                            new ErrorResponse(error.Code, error.Message)
                        ),
                        _ => throw new UnreachableException(),
                    };
                }
            )
            .WithSummary("Atualizar usuário")
            .WithDescription(
                "Atualiza parcialmente os dados de um usuário. Apenas os campos enviados serão alterados."
            )
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<UpdateUserRequest>>();
    }
}
