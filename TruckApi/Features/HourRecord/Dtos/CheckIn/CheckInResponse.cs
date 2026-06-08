namespace TruckApi.Features.HourRecord.Dtos.CheckIn;

public record CheckInResponse(
    string Id,
    string MachineId,
    string MachineCode,
    string OperatorId,
    string OperatorName,
    DateOnly Date,
    DateTimeOffset StartedAt,
    decimal? HourmeterStart,
    string? Notes,
    DateTimeOffset CreatedAt
);
