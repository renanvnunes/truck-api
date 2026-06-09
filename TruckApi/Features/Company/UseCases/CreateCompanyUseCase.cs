using TruckApi.Features.Company.Dtos.CreateCompany;
using TruckApi.Features.Company.Errors;
using TruckApi.Features.Company.Interfaces;
using CompanyEntity = TruckApi.Infrastructure.Database.Entities.Company;

namespace TruckApi.Features.Company.UseCases;

public class CreateCompanyUseCase(ICompanyRepository repository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CompanyEntity>> ExecuteAsync(CreateCompanyRequest request)
    {
        if (await repository.NameExistsAsync(request.Name))
        {
            return Result<CompanyEntity>.Failure(CompanyErrors.NameAlreadyExists);
        }

        var now = DateTimeOffset.UtcNow;

        var company = new CompanyEntity
        {
            Id = IdGenerator.New(),
            Name = request.Name,
            CreatedAt = now,
            UpdatedAt = now,
        };

        var created = await repository.CreateAsync(company);
        await unitOfWork.CommitAsync();
        return Result<CompanyEntity>.Success(created);
    }
}
