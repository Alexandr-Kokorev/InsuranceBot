using InsuranceBot.Domain.Interfaces.Services;
using MediatR;
using Telegram.Bot.Types;

namespace InsuranceBot.Telegram.Interfaces;

public interface IWorkflow
{
    Task HandleUpdate(Update update, IMediator mediator, ITelegramBotService botService, IUserStateService state,
        CancellationToken cancellationToken);
}