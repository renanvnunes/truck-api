namespace TruckApi.Features.HourRecord.Dtos.CheckOut;

public record CheckOutRequest(
    decimal? HourmeterEnd,
    string? Notes
);
