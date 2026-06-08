using TruckApi.Features.HourRecord.Errors;
using TruckApi.Features.HourRecord.Interfaces;
using HourRecordEntity = TruckApi.Infrastructure.Database.Entities.HourRecord;

namespace TruckApi.Features.HourRecord.UseCases;

public class GetHourRecordByIdUseCase(IHourRecordRepository repository, ICurrentUser currentUser)
{
    public async Task<Result<HourRecordEntity>> ExecuteAsync(string id)
    {
        var record = await repository.GetByIdAsync(id);

        if (record is null)
        {
            return Result<HourRecordEntity>.Failure(HourRecordErrors.NotFound);
        }

        if (!currentUser.Session.CanAccessCompany(record.Machine.CompanyId))
        {
            return Result<HourRecordEntity>.Failure(HourRecordErrors.ForbiddenCompany);
        }

        return Result<HourRecordEntity>.Success(record);
    }
}
