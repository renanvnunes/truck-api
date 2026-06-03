using TruckApi.Features.Machine.Errors;
using TruckApi.Features.Machine.Interfaces;
using MachineEntity = TruckApi.Infrastructure.Database.Entities.Machine;

namespace TruckApi.Features.Machine.UseCases;

public class GetMachineByIdUseCase(IMachineRepository repository, ICurrentUser currentUser)
{
    public async Task<Result<MachineEntity>> ExecuteAsync(string id)
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

        return Result<MachineEntity>.Success(machine);
    }
}
