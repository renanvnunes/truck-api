using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.Interface;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<bool> WhatsappExistsAsync(string whatsapp);
    Task<User[]> GetAllAsync(string? cursor, int limit);

    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByWhatsappAsync(string whatsapp);

    Task UpdateAsync(User user);
    Task RemoveAsync(string id);
    Task<bool> WhatsappExistsForOtherUserAsync(string whatsapp, string excludeId);
}
