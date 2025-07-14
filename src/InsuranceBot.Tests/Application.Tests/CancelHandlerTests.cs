using InsuranceBot.Application.Commands;
using InsuranceBot.Application.Handlers;
using InsuranceBot.Domain.Interfaces.Services;
using Moq;

namespace InsuranceBot.Tests.Application.Tests;

public class CancelHandlerTests
{
    [Fact]
    public async Task Handle_ResetsStateAndNotifiesUser()
    {
        Mock<ITelegramBotService> bot = new Mock<ITelegramBotService>();
        Mock<IUserStateService> state = new Mock<IUserStateService>();
        CancelHandler handler = new CancelHandler(bot.Object, state.Object);
        await handler.Handle(new CancelCommand(777), default);
        bot.Verify(b => b.SendTextAsync(777, It.Is<string>(msg => msg.Contains("cancelled"))), Times.Once);
        state.Verify(s => s.ResetStateAsync(777), Times.Once);
    }
}