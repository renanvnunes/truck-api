using Microsoft.EntityFrameworkCore;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Infrastructure.Database;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.Repository;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public Task<User> CreateAsync(User user)
    {
        db.Users.Add(user);
        return Task.FromResult(user);
    }

    public async Task<bool> WhatsappExistsAsync(string whatsapp) =>
        await db.Users.AnyAsync(u => u.Whatsapp == whatsapp);

    public async Task<User[]> GetAllAsync(string? cursor, int limit, string? companyId)
    {
        var query = db.Users.AsQueryable();

        if (companyId is not null)
        {
            query = query.Where(u => u.CompanyId == companyId);
        }

        if (cursor is not null)
        {
            query = query.Where(u => string.Compare(u.Id, cursor) > 0);
        }

        return await query.OrderBy(u => u.Id).Take(limit).ToArrayAsync();
    }

    public async Task<User?> GetByIdAsync(string id) =>
        await db.Users.FindAsync(id);

    public async Task<User?> GetByWhatsappAsync(string whatsapp) =>
        await db.Users.FirstOrDefaultAsync(u => u.Whatsapp == whatsapp);

    public Task UpdateAsync(User user)
    {
        db.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task RemoveAsync(string id)
    {
        var user = await db.Users.FindAsync(id);
        if (user is not null)
        {
            db.Users.Remove(user);
        }
    }

    public async Task<bool> WhatsappExistsForOtherUserAsync(string whatsapp, string excludeId) =>
        await db.Users.AnyAsync(u => u.Whatsapp == whatsapp && u.Id != excludeId);

    public async Task UpdatePasswordAsync(string userId, string newPasswordHash)
    {
        var user = await db.Users.FindAsync(userId);
        if (user is not null)
        {
            user.Password = newPasswordHash;
            user.UpdatedAt = DateTimeOffset.UtcNow;
            db.Users.Update(user);
        }
    }
}
