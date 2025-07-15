using System;
using System.Threading;
using System.Threading.Tasks;
using InsuranceBot.Application.Commands;
using InsuranceBot.Domain.Enums;
using InsuranceBot.Domain.Interfaces.Services;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;

namespace InsuranceBot.Application.Handlers;

public class ConfirmExtractedDataHandler(ITelegramBotService bot, IUserStateService state)
    : IRequestHandler<ConfirmExtractedDataCommand>
{
    public async Task Handle(ConfirmExtractedDataCommand request, CancellationToken ct)
    {
        if (request.PartlyConfirmed)
        {
            await bot.SendTextAsync(request.TelegramUserId,
                "Data confirmed! Please upload next document");
            await state.SetNextStateAsync(request.TelegramUserId, Enum.GetName(UserState.AwaitingDocumentUpload));
        }
        else if (request.Confirmed)
        {
            await bot.SendTextAsync(request.TelegramUserId,
                "Data confirmed! Insurance price is $100. Reply YES to proceed or NO to cancel.");
            
            await state.SetNextStateAsync(request.TelegramUserId,
                Enum.GetName(UserState.AwaitingPriceConfirmation));
        }
        else
        {
            await bot.SendTextAsync(request.TelegramUserId, "Please re-upload the document.");
            await state.SetNextStateAsync(request.TelegramUserId, Enum.GetName(UserState.AwaitingDocumentUpload));
        }
    }
}