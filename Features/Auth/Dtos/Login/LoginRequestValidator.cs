using FluentValidation;

namespace TruckApi.Features.Auth.Dtos.Login;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Whatsapp)
            .NotEmpty()
            .Length(13)
            .WithMessage("Whatsapp deve conter exatamente 13 dígitos. Exemplo: 5511999999999");
        RuleFor(x => x.Password).NotEmpty();
    }
}
