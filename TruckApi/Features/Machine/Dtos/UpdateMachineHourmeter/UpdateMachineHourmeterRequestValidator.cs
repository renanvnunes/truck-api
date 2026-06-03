using FluentValidation;

namespace TruckApi.Features.Machine.Dtos.UpdateMachineHourmeter;

public class UpdateMachineHourmeterRequestValidator : AbstractValidator<UpdateMachineHourmeterRequest>
{
    public UpdateMachineHourmeterRequestValidator()
    {
        RuleFor(x => x.Hourmeter).GreaterThanOrEqualTo(0);
    }
}
