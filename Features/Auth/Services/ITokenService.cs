using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Auth.Services;

public interface ITokenService
{
    string Generate(User user, out TimeSpan expiration);
}
