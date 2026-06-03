using FluentValidation;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Machine.Dtos.CreateMachine;

public class CreateMachineRequestValidator : AbstractValidator<CreateMachineRequest>
{
    private static readonly string ValidTypes = string.Join(", ", Enum.GetNames<MachineType>());

    public CreateMachineRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(v => Enum.TryParse<MachineType>(v, ignoreCase: true, out _))
            .WithMessage($"'Type' deve ser um dos seguintes valores: {ValidTypes}.");
        RuleFor(x => x.Brand).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Model).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Year).InclusiveBetween(1900, DateTime.UtcNow.Year + 1);
        RuleFor(x => x.SerialNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Plate).MaximumLength(20).When(x => x.Plate is not null);
    }
}
