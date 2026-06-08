using TruckApi.Features.Users.Interfaces;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.UseCases;

public class GetAllUsersUseCase(IUserRepository repository, ICurrentUser currentUser)
{
    public async Task<(User[] Users, string? NextCursor)> ExecuteAsync(string? cursor, int limit)
    {
        limit = Math.Clamp(limit, 1, 100);

        var companyId = currentUser.Session.IsAdmin() ? null : currentUser.Session.CompanyId;

        var users = await repository.GetAllAsync(cursor, limit + 1, companyId);

        string? nextCursor = null;
        if (users.Length > limit)
        {
            nextCursor = users[limit - 1].Id;
            users = users[..limit];
        }

        return (users, nextCursor);
    }
}
