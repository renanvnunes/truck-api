using FluentValidation;

namespace TruckApi.Features.Users.CreateUser;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Whatsapp)
            .NotEmpty()
            .Length(13)
            .WithMessage("Whatsapp deve conter exatamente 13 dígitos. Exemplo: 5511999999999");
        RuleFor(x => x.Password).MinimumLength(6).When(x => x.Password is not null);
        RuleFor(x => x.Age).InclusiveBetween(16, 80).When(x => x.Age is not null);
    }
}
