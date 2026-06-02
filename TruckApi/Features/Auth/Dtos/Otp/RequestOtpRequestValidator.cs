using FluentValidation;

namespace TruckApi.Features.Auth.Dtos.Otp;

public class RequestOtpRequestValidator : AbstractValidator<RequestOtpRequest>
{
    public RequestOtpRequestValidator()
    {
        RuleFor(x => x.Whatsapp)
            .NotEmpty()
            .Length(13)
            .WithMessage("Whatsapp deve conter exatamente 13 dígitos. Exemplo: 5511999999999");
    }
}
