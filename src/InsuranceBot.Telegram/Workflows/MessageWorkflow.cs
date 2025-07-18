using InsuranceBot.Application.Commands;
using InsuranceBot.Domain.Enums;
using InsuranceBot.Domain.Interfaces.Services;
using InsuranceBot.Telegram.Helper;
using InsuranceBot.Telegram.Interfaces;
using InsuranceBot.Telegram.Models;
using MediatR;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace InsuranceBot.Telegram.Workflows;

public class MessageWorkflow : IWorkflow
{
    public static List<UploadedUserDocuments> uploadedDocumentsList;
    private static Guid sessionUuid;

    private static readonly Dictionary<string, Func<long, IMediator, string, Update, CancellationToken, Task>>
        CommandHandlers =
            new Dictionary<string, Func<long, IMediator, string, Update, CancellationToken, Task>>(
                StringComparer.OrdinalIgnoreCase)
            {
                ["/start"] = async (userId, mediator, text, update, ct) =>
                    await mediator.Send(new StartCommand(userId), ct),
                ["/start(admin)"] = async (userId, mediator, text, update, ct) =>
                    await mediator.Send(new StartCommand(userId, true), ct),
                ["/cancel"] = async (userId, mediator, text, update, ct) =>
                    await mediator.Send(new CancelCommand(userId), ct),
                ["/resendpolicy"] = async (userId, mediator, text, update, ct) =>
                    await mediator.Send(new ResendPolicyCommand(userId), ct),
                ["/admin summary"] = async (userId, mediator, text, update, ct) =>
                    await mediator.Send(new AdminSummaryCommand(userId), ct),
                ["/generate_policy"] = async (userId, mediator, text, update, ct) =>
                    await mediator.Send(new GeneratePolicyCommand(userId, sessionUuid), ct)
            };

    private static readonly Dictionary<string, Func<long, IMediator, string, Update, CancellationToken, Task>>
        StateHandlers =
            new Dictionary<string, Func<long, IMediator, string, Update, CancellationToken, Task>>(
                StringComparer.OrdinalIgnoreCase)
            {
                [Enum.GetName(UserState.AwaitingDocumentConfirmation)!] = async (userId, mediator, text, update, ct) =>
                {
                    if (text is "yes" or "y")
                    {
                        uploadedDocumentsList.MarkAsConfirmed();
                        bool partlyConfirmed = uploadedDocumentsList.Count == 1
                            ? uploadedDocumentsList.All(x => x.IsConfirmed)
                            : uploadedDocumentsList.Any(x => !x.IsConfirmed);
                        
                        await mediator.Send(
                            new ConfirmExtractedDataCommand(userId,
                                partlyConfirmed,
                                uploadedDocumentsList.All(x => x.IsConfirmed)),
                            ct);
                    }
                    else if (text is "no" or "n")
                    {
                        uploadedDocumentsList.Remove(uploadedDocumentsList.Last());
                        await mediator.Send(new ConfirmExtractedDataCommand(userId, false, false), ct);
                    }
                        
                },
                [Enum.GetName(UserState.AwaitingPriceConfirmation)!] = async (userId, mediator, text, update, ct) =>
                {
                    if (text is "yes" or "y")
                    {
                        uploadedDocumentsList.CleanUploadedDocumentsList();
                        await mediator.Send(new PriceConfirmationCommand(userId, true), ct);
                    }
                    else if (text is "no" or "n")
                        await mediator.Send(new PriceConfirmationCommand(userId, false), ct);
                }
            };

    public async Task HandleUpdate(Update update, IMediator mediator, ITelegramBotService botService,
        IUserStateService state, CancellationToken cancellationToken)
    {
        if (update.Message is { From: not null })
        {
            long userId = update.Message.From.Id;
            string text = (update.Message.Text ?? String.Empty).Trim().ToLower();
            Document? doc = update.Message.Document;

            string userState = await state.GetUserStateAsync(userId, update.Message.Text == "/start(admin)") ?? "Start";

            if (CommandHandlers.TryGetValue(text,
                    out Func<long, IMediator, string, Update, CancellationToken, Task>? cmdHandler))
            {
                if (userState == "Start")
                {
                    sessionUuid = Guid.NewGuid();
                    uploadedDocumentsList = new List<UploadedUserDocuments>();
                }
                
                await cmdHandler(userId, mediator, text, update, cancellationToken);
                return;
            }

            if (doc != null || update.Message.Type == MessageType.Photo)
            {
                string fileId = String.Empty;
                string fileType;

                if (update.Message.Type == MessageType.Photo)
                {
                    PhotoSize[]? photo = update.Message.Photo;
                    fileType = "Upload";
                    if (photo != null) fileId = photo.Last().FileId;
                }
                else
                {
                    fileType = doc.FileName != null && doc.FileName.EndsWith(".pdf") ? "Policy" : "Upload";
                    fileId = doc.FileId;
                }

                uploadedDocumentsList.UpdateUserDocsUploadList(sessionUuid);
                UploadedUserDocuments document = uploadedDocumentsList.GetDocumentTypeToUpload(sessionUuid);

                Stream fileStream = await botService.GetFileStreamAsync(fileId);

                await mediator.Send(new UploadDocumentCommand(userId, fileStream, fileType, document.Type, document.SessionUuid),
                    cancellationToken);

                return;
            }

            if (StateHandlers.TryGetValue(userState,
                    out Func<long, IMediator, string, Update, CancellationToken, Task>? stateHandler))
            {
                await stateHandler(userId, mediator, text, update, cancellationToken);
                return;
            }

            await botService.SendTextAsync(userId, "Sorry, I didn't understand that. Please try again.");
        }
    }
}