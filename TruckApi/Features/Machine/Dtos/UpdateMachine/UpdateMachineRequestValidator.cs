using FluentValidation;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Machine.Dtos.UpdateMachine;

public class UpdateMachineRequestValidator : AbstractValidator<UpdateMachineRequest>
{
    public UpdateMachineRequestValidator()
    {
        RuleFor(x => x.Code).MaximumLength(50).When(x => x.Code is not null);
        RuleFor(x => x.Type)
            .Must(v => Enum.TryParse<MachineType>(v, ignoreCase: true, out _))
            .WithMessage($"'Type' deve ser um dos seguintes valores: {string.Join(", ", Enum.GetNames<MachineType>())}.")
            .When(x => x.Type is not null);
        RuleFor(x => x.Brand).MaximumLength(100).When(x => x.Brand is not null);
        RuleFor(x => x.Model).MaximumLength(100).When(x => x.Model is not null);
        RuleFor(x => x.Year).InclusiveBetween(1900, DateTime.UtcNow.Year + 1).When(x => x.Year is not null);
        RuleFor(x => x.SerialNumber).MaximumLength(100).When(x => x.SerialNumber is not null);
        RuleFor(x => x.Plate).MaximumLength(20).When(x => x.Plate is not null);
    }
}
