using System.Threading.Tasks;
using InsuranceBot.Domain.Entities;

namespace InsuranceBot.Domain.Interfaces.Repositories;

public interface IPolicyRepository
{
    Task SavePolicyAsync(Policy policy);
    Task<Policy> GetLastPolicyAsync(long userId);
    Task<string> GetIssuedPoliciesSummaryAsync();
}