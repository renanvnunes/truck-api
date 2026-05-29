using TruckApi.Features.Company.Errors;
using TruckApi.Features.Company.Interfaces;
using TruckApi.Features.Users.Dtos.CreateUser;
using TruckApi.Features.Users.Errors;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.UseCases;

public class CreateUserUseCase(IUserRepository repository, ICompanyRepository companyRepository)
{
    public async Task<Result<User>> ExecuteAsync(CreateUserRequest request)
    {
        if (await repository.WhatsappExistsAsync(request.Whatsapp))
        {
            return Result<User>.Failure(UserErrors.WhatsappAlreadyExists);
        }

        if (request.CompanyId is not null)
        {
            var companyExists = await companyRepository.ExistsAsync(request.CompanyId);
            if (!companyExists)
            {
                return Result<User>.Failure(CompanyErrors.NotFound);
            }
        }

        var now = DateTimeOffset.UtcNow;

        var passwordHash = request.Password is not null
            ? PasswordHash.Hash(request.Password)
            : null;

        var user = new User
        {
            Id = IdGenerator.New(),
            FullName = request.FullName,
            Whatsapp = request.Whatsapp,
            Password = passwordHash,
            Document = request.Document,
            Age = request.Age,
            Role = request.Role,
            CompanyId = request.CompanyId,
            Timezone = request.Timezone,
            CreatedAt = now,
            UpdatedAt = now,
        };

        return Result<User>.Success(await repository.CreateAsync(user));
    }
}
