using TruckApi.Features.Users.Errors;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.UseCases;

public class DeleteUserUseCase(IUserRepository repository)
{
    public async Task<Result<User>> ExecuteAsync(string id)
    {
        var user = await repository.GetByIdAsync(id);

        if (user is null)
        {
            return Result<User>.Failure(UserErrors.NotFound);
        }

        await repository.RemoveAsync(id);

        return Result<User>.Success(user);
    }
}
