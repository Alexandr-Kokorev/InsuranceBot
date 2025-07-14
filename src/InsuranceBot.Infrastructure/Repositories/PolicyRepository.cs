using System.Linq;
using System.Threading.Tasks;
using InsuranceBot.Domain.Entities;
using InsuranceBot.Domain.Interfaces.Repositories;
using InsuranceBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InsuranceBot.Infrastructure.Repositories;

public class PolicyRepository(AppDbContext db) : IPolicyRepository
{
    public async Task SavePolicyAsync(Policy policy)
    {
        db.Policies.Add(policy);
        await db.SaveChangesAsync();
    }

    public async Task<Policy> GetLastPolicyAsync(long userId)
        => await db.Policies.Where(p => p.UserId == userId).OrderByDescending(p => p.IssuedAt).FirstOrDefaultAsync();

    public async Task<string> GetIssuedPoliciesSummaryAsync()
    {
        int count = await db.Policies.CountAsync();
        Policy last = await db.Policies.OrderByDescending(p => p.IssuedAt).FirstOrDefaultAsync();
        return $"Total: {count}\nLast: {last?.IssuedAt} ({last?.Status})";
    }
}