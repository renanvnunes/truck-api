using Microsoft.EntityFrameworkCore;
using TruckApi.Features.Machine.Interfaces;
using TruckApi.Infrastructure.Database;
using TruckApi.Infrastructure.Database.Entities;
using MachineEntity = TruckApi.Infrastructure.Database.Entities.Machine;

namespace TruckApi.Features.Machine.Repository;

public class MachineRepository(AppDbContext db) : IMachineRepository
{
    public Task<MachineEntity> CreateAsync(MachineEntity machine)
    {
        db.Machines.Add(machine);
        return Task.FromResult(machine);
    }

    public async Task<MachineEntity[]> GetAllAsync(string? cursor, int limit, string? companyId)
    {
        var query = db.Machines.AsQueryable();

        if (companyId is not null)
        {
            query = query.Where(m => m.CompanyId == companyId);
        }

        if (cursor is not null)
        {
            query = query.Where(m => string.Compare(m.Id, cursor) > 0);
        }

        return await query.OrderBy(m => m.Id).Take(limit).ToArrayAsync();
    }

    public async Task<MachineEntity?> GetByIdAsync(string id) =>
        await db.Machines.FindAsync(id);

    public async Task<bool> SerialNumberExistsAsync(string serialNumber) =>
        await db.Machines.AnyAsync(m => m.SerialNumber == serialNumber);

    public async Task<bool> SerialNumberExistsForOtherMachineAsync(
        string serialNumber,
        string excludeId
    ) => await db.Machines.AnyAsync(m => m.SerialNumber == serialNumber && m.Id != excludeId);

    public async Task<bool> CodeExistsInCompanyAsync(string code, string companyId) =>
        await db.Machines.AnyAsync(m => m.Code == code && m.CompanyId == companyId);

    public async Task<bool> CodeExistsForOtherMachineInCompanyAsync(
        string code,
        string companyId,
        string excludeId
    ) =>
        await db.Machines.AnyAsync(m =>
            m.Code == code && m.CompanyId == companyId && m.Id != excludeId
        );

    public Task UpdateAsync(MachineEntity machine)
    {
        db.Machines.Update(machine);
        return Task.CompletedTask;
    }

    public async Task UpdateStatusAsync(string id, MachineStatus status)
    {
        var machine = await db.Machines.FindAsync(id);
        if (machine is not null)
        {
            machine.Status = status;
            machine.UpdatedAt = DateTimeOffset.UtcNow;
        }
    }

    public async Task UpdateHourmeterAsync(string id, decimal hourmeter)
    {
        var machine = await db.Machines.FindAsync(id);
        if (machine is not null)
        {
            machine.CurrentHourmeter = hourmeter;
            machine.UpdatedAt = DateTimeOffset.UtcNow;
        }
    }

    public async Task RemoveAsync(string id)
    {
        var machine = await db.Machines.FindAsync(id);
        if (machine is not null)
        {
            db.Machines.Remove(machine);
        }
    }
}
