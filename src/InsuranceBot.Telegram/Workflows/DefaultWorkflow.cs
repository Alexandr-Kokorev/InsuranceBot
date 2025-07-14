using InsuranceBot.Domain.Interfaces.Services;
using InsuranceBot.Telegram.Interfaces;
using MediatR;
using Telegram.Bot.Types;

namespace InsuranceBot.Telegram.Workflows;

public class DefaultWorkflow : IWorkflow
{
    public async Task HandleUpdate(Update update, IMediator mediator, ITelegramBotService botService,
        IUserStateService state, CancellationToken cancellationToken)
    {
        if (update.Message is { From: not null })
        {
            long userId = update.Message.From.Id;
            await botService.SendTextAsync(userId, "Sorry, I didn't understand that. Please try again.");
        }
    }
}