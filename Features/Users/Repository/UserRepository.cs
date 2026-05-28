using Microsoft.EntityFrameworkCore;
using TruckApi.Features.Users.Interface;
using TruckApi.Infrastructure.Database;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.Repository;

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

    public async Task<User[]> GetAllAsync()
    {
        return await db.Users.ToArrayAsync();
    }
}
