using TruckApi.Features.Machine.Dtos.UpdateMachine;
using TruckApi.Features.Machine.Errors;
using TruckApi.Features.Machine.Interfaces;
using TruckApi.Infrastructure.Database.Entities;
using MachineEntity = TruckApi.Infrastructure.Database.Entities.Machine;

namespace TruckApi.Features.Machine.UseCases;

public class UpdateMachineUseCase(IMachineRepository repository, ICurrentUser currentUser)
{
    public async Task<Result<MachineEntity>> ExecuteAsync(string id, UpdateMachineRequest request)
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

        if (request.SerialNumber is not null && request.SerialNumber != machine.SerialNumber)
        {
            if (await repository.SerialNumberExistsForOtherMachineAsync(request.SerialNumber, id))
            {
                return Result<MachineEntity>.Failure(MachineErrors.SerialNumberAlreadyExists);
            }

            machine.SerialNumber = request.SerialNumber;
        }

        if (request.Code is not null && request.Code != machine.Code)
        {
            if (
                await repository.CodeExistsForOtherMachineInCompanyAsync(
                    request.Code,
                    machine.CompanyId,
                    id
                )
            )
                return Result<MachineEntity>.Failure(MachineErrors.CodeAlreadyExistsInCompany);

            machine.Code = request.Code;
        }

        if (request.Type is not null)
        {
            machine.Type = Enum.Parse<MachineType>(request.Type, ignoreCase: true);
        }
        if (request.Brand is not null)
        {
            machine.Brand = request.Brand;
        }
        if (request.Model is not null)
        {
            machine.Model = request.Model;
        }
        if (request.Year is not null)
        {
            machine.Year = request.Year.Value;
        }
        if (request.Plate is not null)
        {
            machine.Plate = request.Plate;
        }

        machine.UpdatedAt = DateTimeOffset.UtcNow;

        await repository.UpdateAsync(machine);

        return Result<MachineEntity>.Success(machine);
    }
}
