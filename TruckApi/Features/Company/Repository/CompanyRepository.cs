using Microsoft.EntityFrameworkCore;
using TruckApi.Features.Company.Interfaces;
using TruckApi.Infrastructure.Database;
using CompanyEntity = TruckApi.Infrastructure.Database.Entities.Company;

namespace TruckApi.Features.Company.Repository;

public class CompanyRepository(AppDbContext db) : ICompanyRepository
{
    public Task<CompanyEntity> CreateAsync(CompanyEntity company)
    {
        db.Companies.Add(company);
        return Task.FromResult(company);
    }

    public async Task<bool> NameExistsAsync(string name) =>
        await db.Companies.AnyAsync(c => c.Name == name);

    public async Task<CompanyEntity[]> GetAllAsync(string? cursor, int limit)
    {
        var query = db.Companies.AsQueryable();

        if (cursor is not null)
        {
            query = query.Where(c => string.Compare(c.Id, cursor) > 0);
        }

        return await query.OrderBy(c => c.Id).Take(limit).ToArrayAsync();
    }

    public async Task<CompanyEntity?> GetByIdAsync(string id) =>
        await db.Companies.FindAsync(id);

    public Task UpdateAsync(CompanyEntity company)
    {
        db.Companies.Update(company);
        return Task.CompletedTask;
    }

    public async Task RemoveAsync(string id)
    {
        var company = await db.Companies.FindAsync(id);
        if (company is not null)
        {
            db.Companies.Remove(company);
        }
    }

    public async Task<bool> NameExistsForOtherCompanyAsync(string name, string excludeId) =>
        await db.Companies.AnyAsync(c => c.Name == name && c.Id != excludeId);

    public async Task<bool> ExistsAsync(string id) =>
        await db.Companies.AnyAsync(c => c.Id == id);
}
