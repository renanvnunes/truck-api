using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<bool> WhatsappExistsAsync(string whatsapp);
}
