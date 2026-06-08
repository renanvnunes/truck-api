namespace TruckApi.Features.HourRecord.Dtos.CheckOut;

public record CheckOutResponse(
    string Id,
    string MachineId,
    string OperatorId,
    DateOnly Date,
    DateTimeOffset StartedAt,
    DateTimeOffset EndedAt,
    decimal TotalHours,
    decimal? HourmeterStart,
    decimal? HourmeterEnd,
    string? Notes,
    DateTimeOffset UpdatedAt
);
