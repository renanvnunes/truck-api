namespace TruckApi.Features.Machine.Dtos.CreateMachine;

/// <summary>Dados para cadastro de uma nova máquina.</summary>
public record CreateMachineRequest(
    string Code,
    string Type,
    string Brand,
    string Model,
    int Year,
    string SerialNumber,
    string? Plate,
    string? CompanyId
);
