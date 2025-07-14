using System.Threading;
using System.Threading.Tasks;
using InsuranceBot.Application.Commands;
using InsuranceBot.Domain.Interfaces.Services;
using MediatR;

namespace InsuranceBot.Application.Handlers;

public class CancelHandler(ITelegramBotService bot, IUserStateService state) : IRequestHandler<CancelCommand>
{
    public async Task Handle(CancelCommand request, CancellationToken ct)
    {
        await state.ResetStateAsync(request.TelegramUserId);
        await bot.SendTextAsync(request.TelegramUserId, "Flow cancelled. Type /start to begin again.");
    }
}