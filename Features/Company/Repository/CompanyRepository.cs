using Microsoft.EntityFrameworkCore;
using TruckApi.Features.Company.Interface;
using TruckApi.Infrastructure.Database;
using CompanyEntity = TruckApi.Infrastructure.Database.Entities.Company;

namespace TruckApi.Features.Company.Repository;

public class CompanyRepository(AppDbContext db) : ICompanyRepository
{
    public async Task<CompanyEntity> CreateAsync(CompanyEntity company)
    {
        db.Companies.Add(company);
        await db.SaveChangesAsync();
        return company;
    }

    public async Task<bool> NameExistsAsync(string name)
    {
        return await db.Companies.AnyAsync(c => c.Name == name);
    }

    public async Task<CompanyEntity[]> GetAllAsync(string? cursor, int limit)
    {
        var query = db.Companies.AsQueryable();

        if (cursor is not null)
            query = query.Where(c => string.Compare(c.Id, cursor) > 0);

        return await query.OrderBy(c => c.Id).Take(limit).ToArrayAsync();
    }

    public async Task<CompanyEntity?> GetByIdAsync(string id)
    {
        return await db.Companies.FindAsync(id);
    }

    public async Task UpdateAsync(CompanyEntity company)
    {
        db.Companies.Update(company);
        await db.SaveChangesAsync();
    }

    public async Task RemoveAsync(string id)
    {
        var company = await db.Companies.FindAsync(id);
        if (company is not null)
        {
            db.Companies.Remove(company);
            await db.SaveChangesAsync();
        }
    }

    public async Task<bool> NameExistsForOtherCompanyAsync(string name, string excludeId)
    {
        return await db.Companies.AnyAsync(c => c.Name == name && c.Id != excludeId);
    }
}
