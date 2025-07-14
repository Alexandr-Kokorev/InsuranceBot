using System.Threading.Tasks;
using InsuranceBot.Domain.Entities;

namespace InsuranceBot.Domain.Interfaces.Services;

public interface IErrorLogService
{
    Task LogAsync(ErrorLog err);
    Task<string> GetRecentErrorsAsync(int count = 5);
}