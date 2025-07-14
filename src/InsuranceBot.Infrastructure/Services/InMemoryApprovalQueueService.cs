using System.Threading.Channels;
using System.Threading.Tasks;
using InsuranceBot.Domain.Entities;
using InsuranceBot.Domain.Interfaces.Services;

namespace InsuranceBot.Infrastructure.Services;

public class InMemoryApprovalQueueService : IApprovalQueueService
{
    private readonly Channel<PolicyApprovalTask> _queue = Channel.CreateUnbounded<PolicyApprovalTask>();
    public Task EnqueueAsync(PolicyApprovalTask task) => _queue.Writer.WriteAsync(task).AsTask();
    public Task<PolicyApprovalTask> DequeueAsync() => _queue.Reader.ReadAsync().AsTask();
}