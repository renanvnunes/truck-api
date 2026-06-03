using TruckApi.Features.Company.Errors;
using TruckApi.Features.Company.Interfaces;
using TruckApi.Features.Machine.Dtos.CreateMachine;
using TruckApi.Features.Machine.Errors;
using TruckApi.Features.Machine.Interfaces;
using TruckApi.Infrastructure.Database.Entities;
using MachineEntity = TruckApi.Infrastructure.Database.Entities.Machine;

namespace TruckApi.Features.Machine.UseCases;

public class CreateMachineUseCase(
    IMachineRepository machineRepository,
    ICompanyRepository companyRepository,
    ICurrentUser currentUser
)
{
    public async Task<Result<MachineEntity>> ExecuteAsync(CreateMachineRequest request)
    {
        var companyId = currentUser.Session.IsAdmin() ? request.CompanyId : currentUser.Session.CompanyId;

        if (companyId is null)
        {
            return Result<MachineEntity>.Failure(MachineErrors.CompanyIdRequired);
        }

        if (!await companyRepository.ExistsAsync(companyId))
        {
            return Result<MachineEntity>.Failure(CompanyErrors.NotFound);
        }

        if (await machineRepository.SerialNumberExistsAsync(request.SerialNumber))
        {
            return Result<MachineEntity>.Failure(MachineErrors.SerialNumberAlreadyExists);
        }

        if (await machineRepository.CodeExistsInCompanyAsync(request.Code, companyId))
        {
            return Result<MachineEntity>.Failure(MachineErrors.CodeAlreadyExistsInCompany);
        }

        var now = DateTimeOffset.UtcNow;
        var machine = new MachineEntity
        {
            Id = IdGenerator.New(),
            Code = request.Code,
            Type = Enum.Parse<MachineType>(request.Type, ignoreCase: true),
            Brand = request.Brand,
            Model = request.Model,
            Year = request.Year,
            SerialNumber = request.SerialNumber,
            Plate = request.Plate,
            CompanyId = companyId,
            CreatedAt = now,
            UpdatedAt = now,
        };

        return Result<MachineEntity>.Success(await machineRepository.CreateAsync(machine));
    }
}
