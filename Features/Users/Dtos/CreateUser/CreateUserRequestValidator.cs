using FluentValidation;

namespace TruckApi.Features.Users.Dtos.CreateUser;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Whatsapp)
            .NotEmpty()
            .Length(13)
            .WithMessage("Whatsapp deve conter exatamente 13 dígitos. Exemplo: 5511999999999");
        RuleFor(x => x.Password).Length(6, 30).NotEmpty();
        RuleFor(x => x.Age).InclusiveBetween(18, 90).When(x => x.Age is not null);
        RuleFor(x => x.Role).IsInEnum();
    }
}
