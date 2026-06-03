using Carter;
using TruckApi.Extensions;
using TruckApi.Features.Auth.Dtos.Refresh;
using TruckApi.Features.Auth.UseCases;

namespace TruckApi.Features.Auth.Endpoints;

public class RefreshToken : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/auth")
            .WithTags("Auth")
            .MapPost(
                "/refresh",
                (RefreshTokenRequest request, RefreshTokenUseCase useCase) =>
                    useCase.ExecuteAsync(request).ToHttpResultAsync()
            )
            .WithSummary("Renovar token")
            .WithDescription("Valida o refresh token, emite um novo access token e rotaciona o refresh token.")
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<RefreshTokenRequest>>()
            .RequireRateLimiting(RateLimitExtensions.Policy.Auth);
    }
}
