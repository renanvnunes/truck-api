using TruckApi.Features.Users.Dtos.CreateUser;
using TruckApi.Features.Users.Errors;
using TruckApi.Features.Users.Interface;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.UseCases;

public class CreateUserUseCase(IUserRepository repository)
{
    public async Task<Result<User>> ExecuteAsync(CreateUserRequest request)
    {
        if (await repository.WhatsappExistsAsync(request.Whatsapp))
        {
            return Result<User>.Failure(UserErrors.WhatsappAlreadyExists);
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
