using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using InsuranceBot.Application.Commands;
using InsuranceBot.Domain.Entities;
using InsuranceBot.Domain.Enums;
using InsuranceBot.Domain.Interfaces.Repositories;
using InsuranceBot.Domain.Interfaces.Services;
using MediatR;

namespace InsuranceBot.Application.Handlers;

public class GeneratePolicyHandler(
    IPolicyPdfService pdf,
    IFileStorageService storage,
    ITelegramBotService bot,
    IOpenAiService openAi,
    IPolicyRepository policies,
    IUserRepository users,
    IDocumentRepository docs,
    IUserStateService state)
    : IRequestHandler<GeneratePolicyCommand>
{
    public async Task Handle(GeneratePolicyCommand request, CancellationToken ct)
    {
        User user = await users.GetAsync(request.TelegramUserId);
        Dictionary<string, string> userData =
            await docs.GetExtractedFieldsAsync(user.TelegramUserId, request.SessionUuid);
        
        DateTime expiry = DateTime.UtcNow.AddDays(7);
        userData.Add("ExpiryDate", expiry.ToString("yyyy-MM-dd"));
        
        byte[] pdfBytes = pdf.GeneratePolicyPdfAsync(userData);
        
        string filePath = await storage.SavePolicyPdfAsync(request.TelegramUserId, pdfBytes);
        
        await policies.SavePolicyAsync(new Policy
        {
            UserId = user.TelegramUserId, FilePath = filePath, IssuedAt = DateTime.UtcNow, ExpiresAt = expiry, Status = "Issued"
        });
        
        await bot.SendDocumentAsync(request.TelegramUserId, File.OpenRead(filePath), "policy.pdf");
        await bot.SendTextAsync(request.TelegramUserId, $"Policy issued. Expires: {expiry:yyyy-MM-dd}");
        
        await state.SetNextStateAsync(request.TelegramUserId,
            Enum.GetName(UserState.Start));
    }
}