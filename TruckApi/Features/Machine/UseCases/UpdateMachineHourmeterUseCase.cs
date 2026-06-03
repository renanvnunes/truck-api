using TruckApi.Features.Machine.Dtos.UpdateMachineHourmeter;
using TruckApi.Features.Machine.Errors;
using TruckApi.Features.Machine.Interfaces;
using MachineEntity = TruckApi.Infrastructure.Database.Entities.Machine;

namespace TruckApi.Features.Machine.UseCases;

public class UpdateMachineHourmeterUseCase(IMachineRepository repository, ICurrentUser currentUser)
{
    public async Task<Result<MachineEntity>> ExecuteAsync(
        string id,
        UpdateMachineHourmeterRequest request
    )
    {
        var machine = await repository.GetByIdAsync(id);

        if (machine is null)
        {
            return Result<MachineEntity>.Failure(MachineErrors.NotFound);
        }

        if (!currentUser.Session.CanAccessCompany(machine.CompanyId))
        {
            return Result<MachineEntity>.Failure(MachineErrors.Forbidden);
        }

        if (request.Hourmeter < machine.CurrentHourmeter)
            return Result<MachineEntity>.Failure(
                new Error(
                    "Machine.HourmeterCannotDecrease",
                    "O horímetro não pode ser menor que o valor atual.",
                    StatusCodes.Status400BadRequest
                )
            );

        await repository.UpdateHourmeterAsync(id, request.Hourmeter);

        machine.CurrentHourmeter = request.Hourmeter;
        machine.UpdatedAt = DateTimeOffset.UtcNow;

        return Result<MachineEntity>.Success(machine);
    }
}
