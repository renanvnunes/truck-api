using FluentValidation;

namespace TruckApi.Features.Auth.Dtos.ForgotPassword;

public class ConfirmNewPasswordRequestValidator : AbstractValidator<ConfirmNewPasswordRequest>
{
    public ConfirmNewPasswordRequestValidator()
    {
        RuleFor(x => x.Whatsapp)
            .NotEmpty()
            .Length(13)
            .WithMessage("Whatsapp deve conter exatamente 13 dígitos. Exemplo: 5511999999999");
        RuleFor(x => x.Code)
            .NotEmpty()
            .Length(6)
            .WithMessage("Código deve conter exatamente 6 dígitos.");
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(50)
            .WithMessage("A nova senha deve conter no mínimo 6 caracteres.");
    }
}
