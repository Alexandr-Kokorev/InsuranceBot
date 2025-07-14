using System;
using System.Threading;
using System.Threading.Tasks;
using InsuranceBot.Application.Commands;
using InsuranceBot.Domain.Enums;
using InsuranceBot.Domain.Interfaces.Repositories;
using InsuranceBot.Domain.Interfaces.Services;
using MediatR;

namespace InsuranceBot.Application.Handlers;

public class StartCommandHandler(
    ITelegramBotService botService,
    IAuditLogService auditLog,
    IUserRepository users,
    IUserStateService state,
    IOpenAiService openAi)
    : IRequestHandler<StartCommand>
{
    public async Task Handle(StartCommand request, CancellationToken ct)
    {
        string greeting = await openAi.GetSmartReplyAsync(
            "Create short greeting to perform Car Insurance Bot to user\nSteps:\n 1. Upload Passport\n 2. Upload Vehicle Registration\n 3. Confirm Data\n 4. Confirm $100 Payment\n 5. Get PDF Policy.",
            "Welcome to the Car Insurance Bot!\nSteps:\n 1. Upload Passport\n 2. Upload Vehicle Registration\n 3. Confirm Data\n 4. Confirm $100 Payment\n 5. Get PDF Policy.");

        await users.EnsureUserAsync(request.TelegramUserId);
        await botService.SendTextAsync(request.TelegramUserId, greeting);

        await botService.SendTextAsync(
            request.TelegramUserId,
            "Please select, upload passport document of photo:");
        
        await state.SetNextStateAsync(request.TelegramUserId, Enum.GetName(UserState.AwaitingDocumentUpload));

        await auditLog.LogAsync(request.TelegramUserId, "Start", "Started insurance flow");
    }
}