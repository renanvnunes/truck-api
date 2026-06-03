using TruckApi.Features.Machine.Errors;
using TruckApi.Features.Machine.Interfaces;

namespace TruckApi.Features.Machine.UseCases;

public class DeleteMachineUseCase(IMachineRepository repository)
{
    public async Task<Result<bool>> ExecuteAsync(string id)
    {
        var machine = await repository.GetByIdAsync(id);

        if (machine is null)
            return Result<bool>.Failure(MachineErrors.NotFound);

        await repository.RemoveAsync(id);

        return Result<bool>.Success(true);
    }
}
