using InsuranceBot.Domain.Interfaces.Services;
using InsuranceBot.Telegram.Interfaces;
using MediatR;
using Telegram.Bot.Types;

namespace InsuranceBot.Telegram.Handlers;

public class TelegramUpdateHandler(IUserStateService state, ITelegramBotService botService, IMediator mediator)
{
    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        if (update.Message is null || update.Message.From is null)
            return;
        
        Workflow workflow = new Workflow();
        IWorkflow workflowStrategy = workflow.GetWorkflow(update);
        
        await workflowStrategy.HandleUpdate(update, mediator, botService, state, cancellationToken);
    }
}