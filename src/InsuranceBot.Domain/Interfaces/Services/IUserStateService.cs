using System.Threading.Tasks;

namespace InsuranceBot.Domain.Interfaces.Services;

public interface IUserStateService
{
    Task SetNextStateAsync(long userId, string state);
    Task ResetStateAsync(long userId);
    Task<string?> GetUserStateAsync(long telegramUserId, bool isAdmin = false);
}