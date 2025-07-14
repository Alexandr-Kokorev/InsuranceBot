using System;
using System.Threading.Tasks;
using InsuranceBot.Domain.Entities;
using InsuranceBot.Domain.Interfaces.Repositories;
using InsuranceBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InsuranceBot.Infrastructure.Repositories;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public async Task<User> GetAsync(long userId)
        => await db.Users.FirstOrDefaultAsync(u => u.TelegramUserId == userId);

    public async Task EnsureUserAsync(long userId)
    {
        if (await GetAsync(userId) == null)
        {
            db.Users.Add(new User { TelegramUserId = userId, CreatedAt = DateTime.UtcNow, CurrentState = "Start" });
            await db.SaveChangesAsync();
        }
    }

    public async Task IncrementUploadAttemptsAsync(long userId)
    {
        User user = await GetAsync(userId);
        if (user != null) user.UploadAttempts++;
        await db.SaveChangesAsync();
    }
}