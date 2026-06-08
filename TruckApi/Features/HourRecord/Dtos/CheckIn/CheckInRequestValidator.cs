using FluentValidation;

namespace TruckApi.Features.HourRecord.Dtos.CheckIn;

public class CheckInRequestValidator : AbstractValidator<CheckInRequest>
{
    public CheckInRequestValidator()
    {
        RuleFor(x => x.MachineId)
            .NotEmpty().WithMessage("O ID da máquina é obrigatório.");

        RuleFor(x => x.HourmeterStart)
            .GreaterThanOrEqualTo(0).WithMessage("O horímetro inicial deve ser maior ou igual a zero.")
            .When(x => x.HourmeterStart.HasValue);
    }
}
