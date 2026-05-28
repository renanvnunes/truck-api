using FluentValidation;

namespace TruckApi.Features.Users.Dtos.UpdateUser;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.FullName).MaximumLength(100).When(x => x.FullName is not null);
        RuleFor(x => x.Whatsapp)
            .Length(13)
            .WithMessage("Whatsapp deve conter exatamente 13 dígitos. Exemplo: 5511999999999")
            .When(x => x.Whatsapp is not null);
        RuleFor(x => x.Password).MinimumLength(6).When(x => x.Password is not null);
        RuleFor(x => x.Age).InclusiveBetween(18, 90).When(x => x.Age is not null);
    }
}
