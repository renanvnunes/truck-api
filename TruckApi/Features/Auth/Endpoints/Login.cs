using Carter;
using TruckApi.Extensions;
using TruckApi.Features.Auth.Dtos.Login;
using TruckApi.Features.Auth.UseCases;

namespace TruckApi.Features.Auth.Endpoints;

public class Login : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/auth")
            .WithTags("Auth")
            .MapPost(
                "/login",
                (LoginRequest request, LoginUseCase useCase) =>
                    useCase.ExecuteAsync(request).ToHttpResultAsync()
            )
            .WithSummary("Login")
            .WithDescription(
                "Autentica o usuário com whatsapp e senha. Retorna um JWT e os dados da sessão."
            )
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<LoginRequest>>()
            .RequireRateLimiting(RateLimitExtensions.Policy.Login);
    }
}
