using System;
using System.Threading;
using System.Threading.Tasks;
using InsuranceBot.Application.Commands;
using InsuranceBot.Domain.Enums;
using InsuranceBot.Domain.Interfaces.Services;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;

namespace InsuranceBot.Application.Handlers;

public class PriceConfirmationHandler(ITelegramBotService bot, IUserStateService state)
    : IRequestHandler<PriceConfirmationCommand>
{
    public async Task Handle(PriceConfirmationCommand request, CancellationToken ct)
    {
        if (request.Confirmed)
        {
            await bot.SendReplyMarkupAsync(request.TelegramUserId, "Your policy is ready to be issued.",
                new InlineKeyboardMarkup([
                    [
                        InlineKeyboardButton.WithCallbackData("Generate Policy", "generate_policy")
                    ]
                ])
            );
            await state.SetNextStateAsync(request.TelegramUserId, Enum.GetName(UserState.PolicyGeneratingPending));
        }
        else
        {
            await bot.SendTextAsync(request.TelegramUserId, "The price is fixed at $100. Reply YES to accept.");
            await state.SetNextStateAsync(request.TelegramUserId, Enum.GetName(UserState.AwaitingPriceConfirmation));
        }
    }
}