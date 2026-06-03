using Carter;
using TruckApi.Extensions;
using TruckApi.Features.Auth.Dtos.ForgotPassword;
using TruckApi.Features.Auth.UseCases;

namespace TruckApi.Features.Auth.Endpoints;

public class ConfirmNewPassword : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/auth")
            .WithTags("Auth")
            .MapPost(
                "/confirm-new-password",
                (ConfirmNewPasswordRequest request, ConfirmNewPasswordUseCase useCase) =>
                    useCase.ExecuteAsync(request).ToHttpResultAsync()
            )
            .WithSummary("Confirmação de nova senha")
            .WithDescription("Confirma a nova senha do usuário.")
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<ConfirmNewPasswordRequest>>()
            .RequireRateLimiting(RateLimitExtensions.Policy.Auth);
    }
}
