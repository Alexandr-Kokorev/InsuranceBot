using System;
using System.Threading.Tasks;
using InsuranceBot.Domain.Entities;
using InsuranceBot.Domain.Interfaces.Repositories;
using InsuranceBot.Domain.Interfaces.Services;
using InsuranceBot.Infrastructure.Data;

namespace InsuranceBot.Infrastructure.Services;

public class UserStateService(AppDbContext db, IUserRepository userRepository) : IUserStateService
{
    public async Task SetNextStateAsync(long userId, string state)
    {
        User user = await userRepository.GetAsync(userId);
        if (user != null)
        {
            user.CurrentState = state;
            user.UpdatedAt = DateTime.UtcNow;
        }

        await db.SaveChangesAsync();
    }

    public async Task ResetStateAsync(long userId)
    {
        User user = await userRepository.GetAsync(userId);
        user.CurrentState = "Start";
        user.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }
    
    public async Task<string?> GetUserStateAsync(long telegramUserId, bool isAdmin = false)
    {
        User user = await userRepository.GetAsync(telegramUserId);
        if (user != null)
            return user.CurrentState;

        await userRepository.EnsureUserAsync(telegramUserId, isAdmin);
        user = await userRepository.GetAsync(telegramUserId);
        return user?.CurrentState;
    }
}