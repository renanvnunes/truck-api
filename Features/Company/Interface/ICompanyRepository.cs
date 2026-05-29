using CompanyEntity = TruckApi.Infrastructure.Database.Entities.Company;

namespace TruckApi.Features.Company.Interface;

public interface ICompanyRepository
{
    Task<CompanyEntity> CreateAsync(CompanyEntity company);
    Task<bool> NameExistsAsync(string name);
    Task<CompanyEntity[]> GetAllAsync(string? cursor, int limit);
    Task<CompanyEntity?> GetByIdAsync(string id);
    Task UpdateAsync(CompanyEntity company);
    Task RemoveAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<bool> NameExistsForOtherCompanyAsync(string name, string excludeId);
}
