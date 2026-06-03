using TruckApi.Features.Machine.Interfaces;
using MachineEntity = TruckApi.Infrastructure.Database.Entities.Machine;

namespace TruckApi.Features.Machine.UseCases;

public class GetAllMachinesUseCase(IMachineRepository repository, ICurrentUser currentUser)
{
    public async Task<(MachineEntity[] Machines, string? NextCursor)> ExecuteAsync(
        string? cursor,
        int limit
    )
    {
        limit = Math.Clamp(limit, 1, 100);

        var companyId = currentUser.Session.IsAdmin() ? null : currentUser.Session.CompanyId;

        var machines = await repository.GetAllAsync(cursor, limit + 1, companyId);

        string? nextCursor = null;
        if (machines.Length > limit)
        {
            nextCursor = machines[limit - 1].Id;
            machines = machines[..limit];
        }

        return (machines, nextCursor);
    }
}
