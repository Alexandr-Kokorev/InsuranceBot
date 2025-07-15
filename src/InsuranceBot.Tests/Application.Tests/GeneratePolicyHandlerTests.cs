using InsuranceBot.Application.Commands;
using InsuranceBot.Application.Handlers;
using InsuranceBot.Domain.Entities;
using InsuranceBot.Domain.Interfaces.Repositories;
using InsuranceBot.Domain.Interfaces.Services;
using Moq;

namespace InsuranceBot.Tests.Application.Tests;

public class GeneratePolicyHandlerTests
{
    [Fact]
    public async Task Handle_GeneratesPdfAndSendsToUser()
    {
        Mock<IPolicyPdfService> pdfService = new Mock<IPolicyPdfService>();
        pdfService.Setup(p => p.GeneratePolicyPdfAsync(It.IsAny<Dictionary<string, string>>(), It.IsAny<DateTime>())).ReturnsAsync(new byte[] { 1, 2, 3 });
        Mock<IFileStorageService> storage = new Mock<IFileStorageService>();
        storage.Setup(s => s.SavePolicyPdfAsync(123, It.IsAny<byte[]>())).ReturnsAsync("/tmp/policy.pdf");
        Mock<ITelegramBotService> bot = new Mock<ITelegramBotService>();
        Mock<IOpenAiService> openai = new Mock<IOpenAiService>();
        openai.Setup(o => o.GeneratePolicyTextAsync(It.IsAny<Dictionary<string, string>>())).ReturnsAsync("AI Policy Text");
        Mock<IPolicyRepository> policies = new Mock<IPolicyRepository>();
        Mock<IUserRepository> users = new Mock<IUserRepository>();
        Mock<IDocumentRepository> docs = new Mock<IDocumentRepository>();
        Mock<IUserStateService> state = new Mock<IUserStateService>();
        users.Setup(u => u.GetAsync(123)).ReturnsAsync(new User { Id = 123 });
        GeneratePolicyHandler handler = new GeneratePolicyHandler(pdfService.Object, storage.Object, bot.Object, openai.Object, policies.Object, users.Object, docs.Object, state.Object);
        await handler.Handle(new GeneratePolicyCommand(123, Guid.NewGuid()), default);
        bot.Verify(b => b.SendDocumentAsync(123, It.IsAny<Stream>(), "policy.pdf"), Times.Once);
        policies.Verify(p => p.SavePolicyAsync(It.IsAny<Policy>()), Times.Once);
    }
}