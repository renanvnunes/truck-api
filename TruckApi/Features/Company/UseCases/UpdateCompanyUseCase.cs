using TruckApi.Features.Company.Dtos.UpdateCompany;
using TruckApi.Features.Company.Errors;
using TruckApi.Features.Company.Interfaces;
using CompanyEntity = TruckApi.Infrastructure.Database.Entities.Company;

namespace TruckApi.Features.Company.UseCases;

public class UpdateCompanyUseCase(ICompanyRepository repository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CompanyEntity>> ExecuteAsync(string id, UpdateCompanyRequest request)
    {
        var company = await repository.GetByIdAsync(id);

        if (company is null)
        {
            return Result<CompanyEntity>.Failure(CompanyErrors.NotFound);
        }

        if (request.Name is not null && request.Name != company.Name)
        {
            if (await repository.NameExistsForOtherCompanyAsync(request.Name, id))
            {
                return Result<CompanyEntity>.Failure(CompanyErrors.NameAlreadyExists);
            }

            company.Name = request.Name;
        }

        company.UpdatedAt = DateTimeOffset.UtcNow;

        await repository.UpdateAsync(company);
        await unitOfWork.CommitAsync();
        return Result<CompanyEntity>.Success(company);
    }
}
