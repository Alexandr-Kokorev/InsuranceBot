using InsuranceBot.Application.Commands;
using InsuranceBot.Application.Handlers;
using InsuranceBot.Domain.Interfaces.Repositories;
using InsuranceBot.Domain.Interfaces.Services;
using Moq;

namespace InsuranceBot.Tests.Application.Tests;

public class StartCommandHandlerTests
{
    [Fact]
    public async Task Handle_SendsGreetingAndAuditLog()
    {
        Mock<ITelegramBotService> bot = new Mock<ITelegramBotService>();
        Mock<IAuditLogService> log = new Mock<IAuditLogService>();
        Mock<IUserRepository> repo = new Mock<IUserRepository>();
        Mock<IUserStateService> state = new Mock<IUserStateService>();
        Mock<IOpenAiService> openai = new Mock<IOpenAiService>();
        StartCommandHandler handler = new StartCommandHandler(bot.Object, log.Object, repo.Object, state.Object, openai.Object);;
        await handler.Handle(new StartCommand(123), default);
        bot.Verify(b => b.SendTextAsync(123, It.Is<string>(msg => msg.Contains("Welcome"))), Times.Once);
        log.Verify(l => l.LogAsync(123, "Start", It.IsAny<string>()), Times.Once);
        repo.Verify(r => r.EnsureUserAsync(123), Times.Once);
    }
}