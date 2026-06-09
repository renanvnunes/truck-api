using Microsoft.EntityFrameworkCore;
using TruckApi.Features.HourRecord.Interfaces;
using TruckApi.Infrastructure.Database;
using HourRecordEntity = TruckApi.Infrastructure.Database.Entities.HourRecord;

namespace TruckApi.Features.HourRecord.Repository;

public class HourRecordRepository(AppDbContext db) : IHourRecordRepository
{
    public async Task<HourRecordEntity> CreateAsync(HourRecordEntity record)
    {
        db.HourRecords.Add(record);
        await db.Entry(record).Reference(h => h.Operator).LoadAsync();
        await db.Entry(record).Reference(h => h.Machine).LoadAsync();
        return record;
    }

    public async Task<HourRecordEntity?> GetByIdAsync(string id) =>
        await db.HourRecords
            .Include(h => h.Machine)
            .Include(h => h.Operator)
            .FirstOrDefaultAsync(h => h.Id == id);

    public async Task<HourRecordEntity?> GetOpenRecordAsync(string operatorId, string machineId) =>
        await db.HourRecords
            .FirstOrDefaultAsync(h =>
                h.OperatorId == operatorId && h.MachineId == machineId && h.EndedAt == null
            );

    public async Task<(HourRecordEntity[] Records, string? NextCursor)> GetAllAsync(
        string? cursor,
        int limit,
        string? machineId,
        string? operatorId,
        DateOnly? date,
        string? companyId
    )
    {
        var query = db.HourRecords
            .Include(h => h.Machine)
            .Include(h => h.Operator)
            .AsQueryable();

        if (companyId is not null)
        {
            query = query.Where(h => h.Machine.CompanyId == companyId);
        }

        if (machineId is not null)
        {
            query = query.Where(h => h.MachineId == machineId);
        }

        if (operatorId is not null)
        {
            query = query.Where(h => h.OperatorId == operatorId);
        }

        if (date.HasValue)
        {
            query = query.Where(h => h.Date == date.Value);
        }

        if (cursor is not null)
        {
            query = query.Where(h => string.Compare(h.Id, cursor) > 0);
        }

        var records = await query.OrderBy(h => h.Id).Take(limit + 1).ToArrayAsync();

        string? nextCursor = null;
        if (records.Length > limit)
        {
            nextCursor = records[limit - 1].Id;
            records = records[..limit];
        }

        return (records, nextCursor);
    }

    public Task UpdateAsync(HourRecordEntity record)
    {
        db.HourRecords.Update(record);
        return Task.CompletedTask;
    }
}
