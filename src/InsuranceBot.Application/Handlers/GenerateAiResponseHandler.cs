using System.Threading;
using System.Threading.Tasks;
using InsuranceBot.Application.Commands;
using InsuranceBot.Domain.Interfaces.Services;
using MediatR;

namespace InsuranceBot.Application.Handlers;

public class GenerateAiResponseHandler(IOpenAiService openAiService, ITelegramBotService telegramBotService) 
    : IRequestHandler<GenerateAiResponseCommand>
{
    public async Task Handle(GenerateAiResponseCommand request, CancellationToken cancellationToken)
    {
        string response = await openAiService.GetSmartReplyAsync(request.Message, "OpenAI currently is not available. Please try again later.");
        await telegramBotService.SendTextAsync(request.UserId, response);
    }
}