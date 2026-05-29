using TruckApi.Features.Company.Dtos.CreateCompany;
using TruckApi.Features.Company.Errors;
using TruckApi.Features.Company.Interface;
using CompanyEntity = TruckApi.Infrastructure.Database.Entities.Company;

namespace TruckApi.Features.Company.UseCases;

public class CreateCompanyUseCase(ICompanyRepository repository)
{
    public async Task<Result<CompanyEntity>> ExecuteAsync(CreateCompanyRequest request)
    {
        if (await repository.NameExistsAsync(request.Name))
            return Result<CompanyEntity>.Failure(CompanyErrors.NameAlreadyExists);

        var now = DateTimeOffset.UtcNow;

        var company = new CompanyEntity
        {
            Id = IdGenerator.New(),
            Name = request.Name,
            CreatedAt = now,
            UpdatedAt = now,
        };

        return Result<CompanyEntity>.Success(await repository.CreateAsync(company));
    }
}
