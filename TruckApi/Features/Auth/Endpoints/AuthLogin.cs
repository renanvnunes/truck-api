using System.Diagnostics;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.Auth.Dtos.Login;
using TruckApi.Features.Auth.Errors;
using TruckApi.Features.Auth.UseCases;

namespace TruckApi.Features.Auth.Endpoints;

public class AuthLogin : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/auth")
            .WithTags("Auth")
            .MapPost(
                "/login",
                async Task<
                    Results<
                        Ok<LoginResponse>,
                        JsonHttpResult<ErrorResponse>,
                        UnauthorizedHttpResult
                    >
                > (LoginRequest request, LoginUseCase useCase) =>
                {
                    return await useCase.ExecuteAsync(request) switch
                    {
                        Result<LoginResponse>.Ok { Value: var response } => TypedResults.Ok(
                            response
                        ),
                        Result<LoginResponse>.Fail { Error: var error }
                            when error == AuthErrors.UserInactive => TypedResults.Json(
                            new ErrorResponse(error.Code, error.Message),
                            statusCode: StatusCodes.Status401Unauthorized
                        ),
                        Result<LoginResponse>.Fail => TypedResults.Unauthorized(),
                        _ => throw new UnreachableException(),
                    };
                }
            )
            .WithSummary("Login")
            .WithDescription(
                "Autentica o usuário com whatsapp e senha. Retorna um JWT e os dados da sessão."
            )
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<LoginRequest>>();
    }
}
