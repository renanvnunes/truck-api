namespace TruckApi.Features.Machine.Dtos.GetAllMachines;

public record GetAllMachinesResponse(GetAllMachinesItem[] Data, string? NextCursor);
