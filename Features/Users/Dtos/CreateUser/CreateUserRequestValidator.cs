using FluentValidation;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.Dtos.CreateUser;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Document).MaximumLength(20);
        RuleFor(x => x.Whatsapp)
            .NotEmpty()
            .Length(13)
            .WithMessage("Whatsapp deve conter exatamente 13 dígitos. Exemplo: 5511999999999");
        RuleFor(x => x.Password).Length(6, 30);
        RuleFor(x => x.Age).InclusiveBetween(18, 90).When(x => x.Age is not null);
        RuleFor(x => x.Role).IsInEnum();

        RuleFor(x => x.CompanyId)
            .Null()
            .When(x => x.Role == UserRole.Admin)
            .WithMessage("Admin não pode pertencer a uma empresa.");

        RuleFor(x => x.CompanyId)
            .NotNull()
            .When(x => x.Role != UserRole.Admin)
            .WithMessage("CompanyId é obrigatório para este perfil.");
    }
}
