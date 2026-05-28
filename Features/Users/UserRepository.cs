using Microsoft.EntityFrameworkCore;
using TruckApi.Infrastructure.Database;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public async Task<User> CreateAsync(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    public async Task<bool> WhatsappExistsAsync(string whatsapp)
    {
        return await db.Users.AnyAsync(u => u.Whatsapp == whatsapp);
    }
}
