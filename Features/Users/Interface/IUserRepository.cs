using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.Interface;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<bool> WhatsappExistsAsync(string whatsapp);
    Task<User[]> GetAllAsync();

    Task<User?> GetByIdAsync(string id);
}
