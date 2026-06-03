using TruckApi.Features.Machine.Dtos.UpdateMachineStatus;
using TruckApi.Features.Machine.Errors;
using TruckApi.Features.Machine.Interfaces;
using MachineEntity = TruckApi.Infrastructure.Database.Entities.Machine;

namespace TruckApi.Features.Machine.UseCases;

public class UpdateMachineStatusUseCase(IMachineRepository repository, ICurrentUser currentUser)
{
    public async Task<Result<MachineEntity>> ExecuteAsync(string id, UpdateMachineStatusRequest request)
    {
        var machine = await repository.GetByIdAsync(id);

        if (machine is null)
            return Result<MachineEntity>.Failure(MachineErrors.NotFound);

        if (!currentUser.Session.CanAccessCompany(machine.CompanyId))
            return Result<MachineEntity>.Failure(MachineErrors.Forbidden);

        await repository.UpdateStatusAsync(id, request.Status);

        machine.Status = request.Status;
        machine.UpdatedAt = DateTimeOffset.UtcNow;

        return Result<MachineEntity>.Success(machine);
    }
}
