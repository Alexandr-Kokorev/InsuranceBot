using System.Threading.Tasks;

namespace InsuranceBot.Domain.Interfaces.Services;

public interface IAuditLogService
{
    Task LogAsync(long userId, string action, string state);
}