using FluentValidation;

namespace TruckApi.Features.HourRecord.Dtos.CheckOut;

public class CheckOutRequestValidator : AbstractValidator<CheckOutRequest>
{
    public CheckOutRequestValidator()
    {
        RuleFor(x => x.HourmeterEnd)
            .GreaterThanOrEqualTo(0).WithMessage("O horímetro final deve ser maior ou igual a zero.")
            .When(x => x.HourmeterEnd.HasValue);
    }
}
