using TruckApi.Features.Company.Errors;
using TruckApi.Features.Company.Interfaces;
using CompanyEntity = TruckApi.Infrastructure.Database.Entities.Company;

namespace TruckApi.Features.Company.UseCases;

public class DeleteCompanyUseCase(ICompanyRepository repository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CompanyEntity>> ExecuteAsync(string id)
    {
        var company = await repository.GetByIdAsync(id);

        if (company is null)
        {
            return Result<CompanyEntity>.Failure(CompanyErrors.NotFound);
        }

        await repository.RemoveAsync(id);
        await unitOfWork.CommitAsync();
        return Result<CompanyEntity>.Success(company);
    }
}
