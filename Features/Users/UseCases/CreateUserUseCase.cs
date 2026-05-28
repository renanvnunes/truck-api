using TruckApi.Infrastructure.Database.Entities;
using TruckApi.Shared;

namespace TruckApi.Features.Users.CreateUser;

public class CreateUserUseCase(IUserRepository repository)
{
    public async Task<User> ExecuteAsync(CreateUserRequest request)
    {
        if (await repository.WhatsappExistsAsync(request.Whatsapp))
        {
            throw new InvalidOperationException("Whatsapp já cadastrado.");
        }

        var now = DateTimeOffset.UtcNow;

        var user = new User
        {
            Id = IdGenerator.New(),
            FullName = request.FullName,
            Whatsapp = request.Whatsapp,
            Password = request.Password is not null
                ? BCrypt.Net.BCrypt.HashPassword(request.Password)
                : null,
            Document = request.Document,
            Age = request.Age,
            Role = request.Role,
            CompanyId = request.CompanyId,
            Timezone = request.Timezone,
            CreatedAt = now,
            UpdatedAt = now,
        };

        return await repository.CreateAsync(user);
    }
}
