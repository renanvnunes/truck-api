using Carter;
using TruckApi.Extensions;
using TruckApi.Features.Auth.Dtos.Otp;
using TruckApi.Features.Auth.UseCases;

namespace TruckApi.Features.Auth.Endpoints;

public class RequestOtp : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/auth")
            .WithTags("Auth")
            .MapPost(
                "/otp/request",
                (RequestOtpRequest request, RequestOtpUseCase useCase) =>
                    useCase.ExecuteAsync(request).ToHttpResultAsync()
            )
            .WithSummary("Solicitar código OTP")
            .WithDescription("Envia um código de login único via WhatsApp.")
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<RequestOtpRequest>>()
            .RequireRateLimiting(RateLimitExtensions.Policy.Auth);
    }
}
