using System.IO;
using System.Threading;
using System.Threading.Tasks;
using InsuranceBot.Application.Commands;
using InsuranceBot.Domain.Entities;
using InsuranceBot.Domain.Interfaces.Repositories;
using InsuranceBot.Domain.Interfaces.Services;
using MediatR;

namespace InsuranceBot.Application.Handlers;

public class ResendPolicyHandler(IPolicyRepository policies, ITelegramBotService bot)
    : IRequestHandler<ResendPolicyCommand>
{
    public async Task Handle(ResendPolicyCommand request, CancellationToken ct)
    {
        Policy policy = await policies.GetLastPolicyAsync(request.TelegramUserId);
        if (policy != null && File.Exists(policy.FilePath))
        {
            await bot.SendDocumentAsync(request.TelegramUserId, File.OpenRead(policy.FilePath), "policy.pdf");
        }
        else
        {
            await bot.SendTextAsync(request.TelegramUserId, "No policy found. Please complete the workflow first.");
        }
    }
}