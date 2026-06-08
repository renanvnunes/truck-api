namespace TruckApi.Features.HourRecord.Dtos.CheckIn;

public record CheckInRequest(
    string MachineId,
    decimal? HourmeterStart,
    string? Notes
);
