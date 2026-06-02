using FluentValidation;

namespace TruckApi.Features.Auth.Dtos.Otp;

public class VerifyOtpRequestValidator : AbstractValidator<VerifyOtpRequest>
{
    public VerifyOtpRequestValidator()
    {
        RuleFor(x => x.Whatsapp)
            .NotEmpty()
            .Length(13)
            .WithMessage("Whatsapp deve conter exatamente 13 dígitos. Exemplo: 5511999999999");

        RuleFor(x => x.Code)
            .NotEmpty()
            .Length(6)
            .WithMessage("Código deve conter exatamente 6 dígitos.");
    }
}
