using TruckApi.Features.HourRecord.Dtos.CheckOut;
using TruckApi.Features.HourRecord.Errors;
using TruckApi.Features.HourRecord.Interfaces;
using TruckApi.Infrastructure.Database.Entities;
using HourRecordEntity = TruckApi.Infrastructure.Database.Entities.HourRecord;

namespace TruckApi.Features.HourRecord.UseCases;

public class CheckOutUseCase(IHourRecordRepository repository, ICurrentUser currentUser)
{
    public async Task<Result<HourRecordEntity>> ExecuteAsync(string id, CheckOutRequest request)
    {
        var record = await repository.GetByIdAsync(id);

        if (record is null)
        {
            return Result<HourRecordEntity>.Failure(HourRecordErrors.NotFound);
        }

        var session = currentUser.Session;
        var isOperator = record.OperatorId == session.Id;
        var isSupervisorOrAbove =
            session.Role == UserRole.Admin.ToString()
            || session.Role == UserRole.CompanyManager.ToString()
            || session.Role == UserRole.CompanySupervisor.ToString();

        if (!isOperator && !isSupervisorOrAbove)
        {
            return Result<HourRecordEntity>.Failure(HourRecordErrors.Forbidden);
        }

        if (!session.CanAccessCompany(record.Machine.CompanyId))
        {
            return Result<HourRecordEntity>.Failure(HourRecordErrors.ForbiddenCompany);
        }

        if (record.EndedAt is not null)
        {
            return Result<HourRecordEntity>.Failure(HourRecordErrors.AlreadyCheckedOut);
        }

        var now = DateTimeOffset.UtcNow;
        var totalHours = (decimal)(now - record.StartedAt).TotalHours;

        record.EndedAt = now;
        record.TotalHours = Math.Round(totalHours, 2);
        record.HourmeterEnd = request.HourmeterEnd;
        record.Notes = request.Notes ?? record.Notes;
        record.UpdatedAt = now;

        await repository.UpdateAsync(record);

        return Result<HourRecordEntity>.Success(record);
    }
}
