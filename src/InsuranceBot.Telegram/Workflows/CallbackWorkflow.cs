using InsuranceBot.Domain.Enums;
using InsuranceBot.Domain.Interfaces.Services;
using InsuranceBot.Telegram.Interfaces;
using MediatR;
using Telegram.Bot.Types;

namespace InsuranceBot.Telegram.Workflows;

public class CallbackWorkflow : IWorkflow
{
    private static DocumentType _documentType;

    private static readonly Dictionary<string, Func<long, IMediator, string, Update, CancellationToken, Task>>
        StateHandlers =
            new Dictionary<string, Func<long, IMediator, string, Update, CancellationToken, Task>>(
                StringComparer.OrdinalIgnoreCase)
            {
            };

    public async Task HandleUpdate(Update update, IMediator mediator, ITelegramBotService botService, IUserStateService state,
        CancellationToken cancellationToken)
    {
        long userId = update.CallbackQuery.Message.From.Id;
        long chatId = update.CallbackQuery.Message.Chat.Id;

        if (update.Message is { From: not null })
        {
            userId = update.Message.From.Id;
            await botService.SendTextAsync(userId, "Sorry, I didn't understand that. Please try again.");
        }
    }
}