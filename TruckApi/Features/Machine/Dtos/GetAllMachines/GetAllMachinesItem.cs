using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Machine.Dtos.GetAllMachines;

public record GetAllMachinesItem(
    string Id,
    string Code,
    MachineType Type,
    string Brand,
    string Model,
    int Year,
    string SerialNumber,
    string? Plate,
    decimal CurrentHourmeter,
    MachineStatus Status,
    string CompanyId,
    DateTimeOffset CreatedAt
);
