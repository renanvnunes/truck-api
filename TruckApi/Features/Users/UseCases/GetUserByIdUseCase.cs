using TruckApi.Features.Users.Errors;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.UseCases;

public class GetUserByIdUseCase(IUserRepository repository, ICurrentUser currentUser)
{
    public async Task<Result<User>> ExecuteAsync(string id)
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

        return Result<User>.Success(user);
    }
}
