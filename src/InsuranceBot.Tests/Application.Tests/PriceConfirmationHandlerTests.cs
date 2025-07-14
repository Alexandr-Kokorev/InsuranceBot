using InsuranceBot.Application.Commands;
using InsuranceBot.Application.Handlers;
using InsuranceBot.Domain.Interfaces.Services;
using Moq;

namespace InsuranceBot.Tests.Application.Tests;

public class PriceConfirmationHandlerTests
{
    [Fact]
    public async Task Handle_Confirmed_AdvancesWorkflow()
    {
        Mock<ITelegramBotService> bot = new Mock<ITelegramBotService>();
        Mock<IUserStateService> state = new Mock<IUserStateService>();
        PriceConfirmationHandler handler = new PriceConfirmationHandler(bot.Object, state.Object);
        await handler.Handle(new PriceConfirmationCommand(123, true), default);
        bot.Verify(b => b.SendTextAsync(123, It.Is<string>(msg => msg.Contains("pending approval"))), Times.Once);
        state.Verify(s => s.SetNextStateAsync(123, "PolicyApprovalPending"), Times.Once);
    }
    [Fact]
    public async Task Handle_NotConfirmed_RepeatsPrice()
    {
        Mock<ITelegramBotService> bot = new Mock<ITelegramBotService>();
        Mock<IUserStateService> state = new Mock<IUserStateService>();
        PriceConfirmationHandler handler = new PriceConfirmationHandler(bot.Object, state.Object);
        await handler.Handle(new PriceConfirmationCommand(123, false), default);
        bot.Verify(b => b.SendTextAsync(123, It.Is<string>(msg => msg.Contains("fixed"))), Times.Once);
        state.Verify(s => s.SetNextStateAsync(123, "AwaitingPriceConfirmation"), Times.Once);
    }
}