namespace TruckApi.Features.Machine.Dtos.UpdateMachine;

/// <summary>Dados para atualização de uma máquina. Apenas os campos informados serão alterados.</summary>
public record UpdateMachineRequest(
    string? Code,
    string? Type,
    string? Brand,
    string? Model,
    int? Year,
    string? SerialNumber,
    string? Plate
);
