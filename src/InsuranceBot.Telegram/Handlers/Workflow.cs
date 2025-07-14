using InsuranceBot.Telegram.Interfaces;
using InsuranceBot.Telegram.Workflows;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace InsuranceBot.Telegram.Handlers;

public class Workflow
{
    public IWorkflow GetWorkflow(Update update)
    {
        return update switch
        {
            { Type: UpdateType.Message, Message: { } msg } => new MessageWorkflow(),
            { Type: UpdateType.CallbackQuery, CallbackQuery: { } cq } => new CallbackWorkflow(),
            _ => new DefaultWorkflow()
        };
    }
}