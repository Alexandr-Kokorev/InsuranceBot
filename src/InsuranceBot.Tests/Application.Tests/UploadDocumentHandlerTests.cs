using InsuranceBot.Application.Commands;
using InsuranceBot.Application.Handlers;
using InsuranceBot.Domain.Enums;
using InsuranceBot.Domain.Interfaces.Repositories;
using InsuranceBot.Domain.Interfaces.Services;
using Moq;

namespace InsuranceBot.Tests.Application.Tests;

public class UploadDocumentHandlerTests
{
    [Fact]
    public async Task Handle_BlocksDuplicateAndPromptsUser()
    {
        MemoryStream file = new MemoryStream(new byte[] { 1, 2 });
        Mock<IFileStorageService> fileStorage = new Mock<IFileStorageService>();
        Mock<IDocumentRepository> docs = new Mock<IDocumentRepository>();
        docs.Setup(d => d.ExistsDuplicateAsync(123, It.IsAny<string>())).ReturnsAsync(true);
        Mock<IMindeeApiService> ocr = new Mock<IMindeeApiService>();
        Mock<ITelegramBotService> bot = new Mock<ITelegramBotService>();
        Mock<IUserRepository> users = new Mock<IUserRepository>();
        Mock<IUserStateService> state = new Mock<IUserStateService>();
        Mock<IOpenAiService> openai = new Mock<IOpenAiService>();
        UploadDocumentHandler handler = new UploadDocumentHandler(fileStorage.Object, docs.Object, ocr.Object, bot.Object, users.Object, openai.Object, state.Object);
        await handler.Handle(new UploadDocumentCommand(123, file, "Upload", DocumentType.Passport, Guid.NewGuid()), default);
        bot.Verify(b => b.SendTextAsync(123, It.Is<string>(s => s.Contains("Duplicate"))), Times.Once);
        docs.Verify(d => d.SaveExtractedFieldsAsync(It.IsAny<long>(), Guid.NewGuid(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_OCR_SavesFields_AndShowsToUser()
    {
        MemoryStream file = new MemoryStream(new byte[] { 3, 4 });
        Mock<IFileStorageService> fileStorage = new Mock<IFileStorageService>();
        fileStorage.Setup(fs => fs.SaveDocumentAsync(123, file, "Upload")).ReturnsAsync("/path/to/file.jpg");
        Mock<IDocumentRepository> docs = new Mock<IDocumentRepository>();
        docs.Setup(d => d.ExistsDuplicateAsync(123, It.IsAny<string>())).ReturnsAsync(false);
        Mock<IMindeeApiService> ocr = new Mock<IMindeeApiService>();
        ocr.Setup(m => m.ExtractPassportFieldsAsync(It.IsAny<string>())).ReturnsAsync(new Dictionary<string, string> { { "Field", "Value" } });
        Mock<ITelegramBotService> bot = new Mock<ITelegramBotService>();
        Mock<IUserRepository> users = new Mock<IUserRepository>();
        Mock<IUserStateService> state = new Mock<IUserStateService>();
        Mock<IOpenAiService> openai = new Mock<IOpenAiService>();
        UploadDocumentHandler handler = new UploadDocumentHandler(fileStorage.Object, docs.Object, ocr.Object, bot.Object, users.Object, openai.Object, state.Object);
        await handler.Handle(new UploadDocumentCommand(123, file, "Upload", DocumentType.Passport, Guid.NewGuid()), default);
        docs.Verify(d => d.SaveExtractedFieldsAsync(123, Guid.NewGuid(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<string>()), Times.Once);
        bot.Verify(b => b.SendTextAsync(123, It.Is<string>(s => s.Contains("OCR data"))), Times.Once);
    }
}