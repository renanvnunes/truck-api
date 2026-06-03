using Carter;
using TruckApi.Extensions;
using TruckApi.Features.Auth.Dtos.Otp;
using TruckApi.Features.Auth.UseCases;

namespace TruckApi.Features.Auth.Endpoints;

public class VerifyOtp : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/auth")
            .WithTags("Auth")
            .MapPost(
                "/otp/verify",
                (VerifyOtpRequest request, VerifyOtpUseCase useCase) =>
                    useCase.ExecuteAsync(request).ToHttpResultAsync()
            )
            .WithSummary("Verificar código OTP")
            .WithDescription("Valida o código OTP e retorna o token de acesso.")
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<VerifyOtpRequest>>()
            .RequireRateLimiting(RateLimitExtensions.Policy.Auth);
    }
}
