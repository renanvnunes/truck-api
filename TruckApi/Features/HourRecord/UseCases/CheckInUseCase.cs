using TruckApi.Features.HourRecord.Dtos.CheckIn;
using TruckApi.Features.HourRecord.Errors;
using TruckApi.Features.HourRecord.Interfaces;
using TruckApi.Features.Machine.Interfaces;
using HourRecordEntity = TruckApi.Infrastructure.Database.Entities.HourRecord;

namespace TruckApi.Features.HourRecord.UseCases;

public class CheckInUseCase(
    IHourRecordRepository repository,
    IMachineRepository machineRepository,
    ICurrentUser currentUser
)
{
    public async Task<Result<HourRecordEntity>> ExecuteAsync(CheckInRequest request)
    {
        Console.WriteLine(currentUser.Session);
        var machine = await machineRepository.GetByIdAsync(request.MachineId);

        if (machine is null)
        {
            return Result<HourRecordEntity>.Failure(HourRecordErrors.MachineNotFound);
        }

        if (!currentUser.Session.CanAccessCompany(machine.CompanyId))
        {
            return Result<HourRecordEntity>.Failure(HourRecordErrors.ForbiddenCompany);
        }

        var openRecord = await repository.GetOpenRecordAsync(
            currentUser.Session.Id,
            request.MachineId
        );

        if (openRecord is not null)
        {
            return Result<HourRecordEntity>.Failure(HourRecordErrors.AlreadyCheckedIn);
        }

        var now = DateTimeOffset.UtcNow;

        var record = new HourRecordEntity
        {
            Id = IdGenerator.New(),
            MachineId = request.MachineId,
            OperatorId = currentUser.Session.Id,
            Date = DateOnly.FromDateTime(now.UtcDateTime),
            StartedAt = now,
            HourmeterStart = request.HourmeterStart,
            Notes = request.Notes,
            CreatedAt = now,
            UpdatedAt = now,
        };

        return Result<HourRecordEntity>.Success(await repository.CreateAsync(record));
    }
}
