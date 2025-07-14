using InsuranceBot.Application.Commands;
using InsuranceBot.Application.Handlers;
using InsuranceBot.Domain.Entities;
using InsuranceBot.Domain.Interfaces.Repositories;
using InsuranceBot.Domain.Interfaces.Services;
using Moq;

namespace InsuranceBot.Tests.Application.Tests;

public class AdminSummaryHandlerTests
{
    [Fact]
    public async Task Handle_SendsSummaryForAdmin()
    {
        Mock<IPolicyRepository> policies = new Mock<IPolicyRepository>();
        policies.Setup(p => p.GetIssuedPoliciesSummaryAsync()).ReturnsAsync("Summary");
        Mock<IErrorLogService> errorLog = new Mock<IErrorLogService>();
        errorLog.Setup(e => e.GetRecentErrorsAsync(5)).ReturnsAsync("No errors");
        Mock<ITelegramBotService> bot = new Mock<ITelegramBotService>();
        Mock<IUserRepository> users = new Mock<IUserRepository>();
        users.Setup(u => u.GetAsync(123)).ReturnsAsync(new User { Id = 123, IsAdmin = true });
        AdminSummaryHandler handler = new AdminSummaryHandler(policies.Object, errorLog.Object, bot.Object, users.Object);
        await handler.Handle(new AdminSummaryCommand(123), default);
        bot.Verify(b => b.SendTextAsync(123, It.Is<string>(msg => msg.Contains("Summary"))), Times.Once);
    }
    [Fact]
    public async Task Handle_RejectsNonAdmin()
    {
        Mock<IPolicyRepository> policies = new Mock<IPolicyRepository>();
        Mock<IErrorLogService> errorLog = new Mock<IErrorLogService>();
        Mock<ITelegramBotService> bot = new Mock<ITelegramBotService>();
        Mock<IUserRepository> users = new Mock<IUserRepository>();
        users.Setup(u => u.GetAsync(111)).ReturnsAsync(new User { Id = 111, IsAdmin = false });
        AdminSummaryHandler handler = new AdminSummaryHandler(policies.Object, errorLog.Object, bot.Object, users.Object);
        await handler.Handle(new AdminSummaryCommand(111), default);
        bot.Verify(b => b.SendTextAsync(111, It.Is<string>(msg => msg.Contains("Not authorized"))), Times.Once);
    }
}