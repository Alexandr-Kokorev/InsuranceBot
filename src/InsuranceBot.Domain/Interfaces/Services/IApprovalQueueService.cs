using System.Threading.Tasks;
using InsuranceBot.Domain.Entities;

namespace InsuranceBot.Domain.Interfaces.Services;

public interface IApprovalQueueService
{
    Task EnqueueAsync(PolicyApprovalTask task);
    Task<PolicyApprovalTask> DequeueAsync();
}