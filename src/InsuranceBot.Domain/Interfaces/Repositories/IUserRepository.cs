using System.Threading.Tasks;
using InsuranceBot.Domain.Entities;

namespace InsuranceBot.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetAsync(long userId);
    Task EnsureUserAsync(long userId);
    Task IncrementUploadAttemptsAsync(long userId);
}