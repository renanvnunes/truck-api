using TruckApi.Features.Users.Dtos.UpdateUser;
using TruckApi.Features.Users.Errors;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.UseCases;

public class UpdateUserUseCase(IUserRepository repository, ICurrentUser currentUser)
{
    public async Task<Result<User>> ExecuteAsync(string id, UpdateUserRequest request)
    {
        var user = await repository.GetByIdAsync(id);

        if (user is null)
        {
            return Result<User>.Failure(UserErrors.NotFound);
        }

        if (!currentUser.Session.CanAccessCompany(user.CompanyId))
        {
            return Result<User>.Failure(UserErrors.Forbidden);
        }

        if (request.Whatsapp is not null && request.Whatsapp != user.Whatsapp)
        {
            if (await repository.WhatsappExistsForOtherUserAsync(request.Whatsapp, id))
            {
                return Result<User>.Failure(UserErrors.WhatsappAlreadyExists);
            }
        }

        if (request.FullName is not null)
        {
            user.FullName = request.FullName;
        }

        if (request.Whatsapp is not null)
        {
            user.Whatsapp = request.Whatsapp;
        }

        if (request.Password is not null)
        {
            user.Password = PasswordHash.Hash(request.Password);
        }

        if (request.Document is not null)
        {
            user.Document = request.Document;
        }

        if (request.Age is not null)
        {
            user.Age = request.Age;
        }

        if (request.Role is not null)
        {
            user.Role = request.Role.Value;
        }

        if (request.Timezone is not null)
        {
            user.Timezone = request.Timezone;
        }

        if (request.IsActive is not null)
        {
            user.IsActive = request.IsActive.Value;
        }

        user.UpdatedAt = DateTimeOffset.UtcNow;

        await repository.UpdateAsync(user);

        return Result<User>.Success(user);
    }
}
