using TruckApi.Features.Users.Errors;
using TruckApi.Features.Users.Interface;
using TruckApi.Infrastructure.Database.Entities;
using TruckApi.Shared;

namespace TruckApi.Features.Users.UseCases;

public class GetUserByIdUseCase(IUserRepository repository)
{
    public async Task<Result<User>> ExecuteAsync(string id)
    {
        var user = await repository.GetByIdAsync(id);

        if (user is null)
        {
            return Result<User>.Failure(UserErrors.NotFound);
        }

        return Result<User>.Success(user);
    }
}
