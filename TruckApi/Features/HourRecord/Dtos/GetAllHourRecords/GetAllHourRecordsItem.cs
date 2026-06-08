namespace TruckApi.Features.HourRecord.Dtos.GetAllHourRecords;

public record GetAllHourRecordsItem(
    string Id,
    string MachineId,
    string MachineCode,
    string OperatorId,
    string OperatorName,
    DateOnly Date,
    DateTimeOffset StartedAt,
    DateTimeOffset? EndedAt,
    decimal? TotalHours,
    decimal? HourmeterStart,
    decimal? HourmeterEnd,
    string? Notes,
    DateTimeOffset CreatedAt
);
