using HourRecordEntity = TruckApi.Infrastructure.Database.Entities.HourRecord;

namespace TruckApi.Features.HourRecord.Interfaces;

public interface IHourRecordRepository
{
    Task<HourRecordEntity> CreateAsync(HourRecordEntity record);
    Task<HourRecordEntity?> GetByIdAsync(string id);
    Task<HourRecordEntity?> GetOpenRecordAsync(string operatorId, string machineId);
    Task<(HourRecordEntity[] Records, string? NextCursor)> GetAllAsync(
        string? cursor,
        int limit,
        string? machineId,
        string? operatorId,
        DateOnly? date,
        string? companyId
    );
    Task UpdateAsync(HourRecordEntity record);
}
