using TruckApi.Features.Company.Interface;
using CompanyEntity = TruckApi.Infrastructure.Database.Entities.Company;

namespace TruckApi.Features.Company.UseCases;

public class GetAllCompaniesUseCase(ICompanyRepository repository)
{
    public async Task<(CompanyEntity[] Companies, string? NextCursor)> ExecuteAsync(
        string? cursor,
        int limit
    )
    {
        limit = Math.Clamp(limit, 1, 100);

        var companies = await repository.GetAllAsync(cursor, limit + 1);

        string? nextCursor = null;
        if (companies.Length > limit)
        {
            nextCursor = companies[limit - 1].Id;
            companies = companies[..limit];
        }

        return (companies, nextCursor);
    }
}
