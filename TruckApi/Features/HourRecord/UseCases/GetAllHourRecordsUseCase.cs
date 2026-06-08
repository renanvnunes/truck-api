using TruckApi.Features.HourRecord.Interfaces;
using HourRecordEntity = TruckApi.Infrastructure.Database.Entities.HourRecord;

namespace TruckApi.Features.HourRecord.UseCases;

public class GetAllHourRecordsUseCase(IHourRecordRepository repository, ICurrentUser currentUser)
{
    public async Task<(HourRecordEntity[] Records, string? NextCursor)> ExecuteAsync(
        string? cursor,
        int limit,
        string? machineId,
        string? operatorId,
        DateOnly? date
    )
    {
        limit = Math.Clamp(limit, 1, 100);

        var companyId = currentUser.Session.IsAdmin() ? null : currentUser.Session.CompanyId;

        return await repository.GetAllAsync(cursor, limit, machineId, operatorId, date, companyId);
    }
}
