namespace TruckApi.Features.HourRecord.Dtos.GetHourRecordById;

public record GetHourRecordByIdResponse(
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
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);
