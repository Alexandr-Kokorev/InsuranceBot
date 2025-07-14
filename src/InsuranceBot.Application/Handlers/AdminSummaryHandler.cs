using System.Threading;
using System.Threading.Tasks;
using InsuranceBot.Application.Commands;
using InsuranceBot.Domain.Entities;
using InsuranceBot.Domain.Interfaces.Repositories;
using InsuranceBot.Domain.Interfaces.Services;
using MediatR;

namespace InsuranceBot.Application.Handlers;

public class AdminSummaryHandler(
    IPolicyRepository policies,
    IErrorLogService errorLog,
    ITelegramBotService bot,
    IUserRepository users)
    : IRequestHandler<AdminSummaryCommand>
{
    public async Task Handle(AdminSummaryCommand request, CancellationToken ct)
    {
        User user = await users.GetAsync(request.TelegramUserId);
        if (!user.IsAdmin)
        {
            await bot.SendTextAsync(request.TelegramUserId, "Not authorized.");
            return;
        }

        string summary = await policies.GetIssuedPoliciesSummaryAsync();
        string errors = await errorLog.GetRecentErrorsAsync();
        
        await bot.SendTextAsync(request.TelegramUserId, $"Summary:\n{summary}\nRecent errors:\n{errors}");
    }
}