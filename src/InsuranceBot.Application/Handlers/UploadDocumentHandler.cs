using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InsuranceBot.Application.Commands;
using InsuranceBot.Application.Helpers;
using InsuranceBot.Domain.Enums;
using InsuranceBot.Domain.Interfaces.Repositories;
using InsuranceBot.Domain.Interfaces.Services;
using MediatR;

namespace InsuranceBot.Application.Handlers;

public class UploadDocumentHandler(
    IFileStorageService fileStorage,
    IDocumentRepository docs,
    IMindeeApiService ocr,
    ITelegramBotService bot,
    IUserRepository users,
    IOpenAiService openAi,
    IUserStateService state)
    : IRequestHandler<UploadDocumentCommand>
{
    public async Task Handle(UploadDocumentCommand request, CancellationToken ct)
    {
        string hash = await CryptographyHelper.ComputeSha256Async(request.FileStream);
        if (await docs.ExistsDuplicateAsync(request.TelegramUserId, hash))
        {
            await bot.SendTextAsync(request.TelegramUserId,
                "Duplicate document detected. Please send a new document.");
            return;
        }

        string path = await fileStorage.SaveDocumentAsync(request.TelegramUserId, request.FileStream, request.Type);

        Dictionary<string, string> fields = new Dictionary<string, string>();
        if (request.DocumentType == DocumentType.Passport)
        {
            fields = await ocr.ExtractPassportFieldsAsync(path);
        }

        if (request.DocumentType == DocumentType.VehiclesLicense)
        {
            fields = await ocr.ExtractVehicleCertificateFieldsAsync(path);
        }

        await docs.SaveExtractedFieldsAsync(request.TelegramUserId, request.SessionUuid, path, fields, hash);
        await users.IncrementUploadAttemptsAsync(request.TelegramUserId);

        string response = await openAi.GetSmartReplyAsync(
            $"Generate notification to user that data was parsed with next fields - {String.Join("\n", fields.Select(f => $"{f.Key}: {f.Value}"))}. Ask to confirm if the data is correct or upload again",
            $"Data parsed from document:\n{String.Join("\n", fields.Select(f => $"{f.Key}: {f.Value}"))}\nReply YES to confirm, or upload again.");
        
        await state.SetNextStateAsync(request.TelegramUserId, Enum.GetName(UserState.AwaitingDocumentConfirmation));

        await bot.SendTextAsync(request.TelegramUserId, response);
    }
}