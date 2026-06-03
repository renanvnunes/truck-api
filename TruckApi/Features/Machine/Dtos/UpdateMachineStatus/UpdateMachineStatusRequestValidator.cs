using FluentValidation;

namespace TruckApi.Features.Machine.Dtos.UpdateMachineStatus;

public class UpdateMachineStatusRequestValidator : AbstractValidator<UpdateMachineStatusRequest>
{
    public UpdateMachineStatusRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}
