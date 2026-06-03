using TruckApi.Infrastructure.Database.Entities;
using MachineEntity = TruckApi.Infrastructure.Database.Entities.Machine;

namespace TruckApi.Features.Machine.Interfaces;

public interface IMachineRepository
{
    Task<MachineEntity> CreateAsync(MachineEntity machine);
    Task<MachineEntity[]> GetAllAsync(string? cursor, int limit, string? companyId);
    Task<MachineEntity?> GetByIdAsync(string id);
    Task<bool> SerialNumberExistsAsync(string serialNumber);
    Task<bool> SerialNumberExistsForOtherMachineAsync(string serialNumber, string excludeId);
    Task<bool> CodeExistsInCompanyAsync(string code, string companyId);
    Task<bool> CodeExistsForOtherMachineInCompanyAsync(
        string code,
        string companyId,
        string excludeId
    );
    Task UpdateAsync(MachineEntity machine);
    Task UpdateStatusAsync(string id, MachineStatus status);
    Task UpdateHourmeterAsync(string id, decimal hourmeter);
    Task RemoveAsync(string id);
}
