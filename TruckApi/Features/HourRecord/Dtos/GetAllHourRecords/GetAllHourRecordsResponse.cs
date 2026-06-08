namespace TruckApi.Features.HourRecord.Dtos.GetAllHourRecords;

public record GetAllHourRecordsResponse(
    GetAllHourRecordsItem[] Items,
    string? NextCursor
);
