using Carter;
using TruckApi.Extensions;
using TruckApi.Features.Auth.Dtos.ForgotPassword;
using TruckApi.Features.Auth.UseCases;

namespace TruckApi.Features.Auth.Endpoints;

public class AuthForgotPassword : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/auth")
            .WithTags("Auth")
            .MapPost(
                "/forgot-password",
                (ForgotPasswordRequest request, ForgotPasswordUseCase useCase) =>
                    useCase.ExecuteAsync(request).ToHttpResultAsync()
            )
            .WithSummary("Recuperação de senha")
            .WithDescription("Envia um código de redefinição de senha para o WhatsApp do usuário.")
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<ForgotPasswordRequest>>()
            .RequireRateLimiting(RateLimitExtensions.Policy.Auth);
    }
}
