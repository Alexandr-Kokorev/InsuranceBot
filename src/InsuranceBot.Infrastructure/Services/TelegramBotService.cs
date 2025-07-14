using System.IO;
using System.Threading.Tasks;
using InsuranceBot.Domain.Interfaces.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace InsuranceBot.Infrastructure.Services;

public class TelegramBotService(ITelegramBotClient client) : ITelegramBotService
{
    public async Task SendTextAsync(long chatId, string message)
        => await client.SendMessage(chatId: chatId, text: message);

    public async Task SendReplyMarkupAsync(long chatId, string text, InlineKeyboardMarkup markup)
        => await client.SendMessage(chatId: chatId, text: text, replyMarkup: markup);

    public async Task SendDocumentAsync(long chatId, Stream docStream, string fileName)
        => await client.SendDocument(chatId: chatId, document: new InputFileStream(docStream, fileName));

    public async Task<Stream> GetFileStreamAsync(string fileId)
    {
        TGFile file = await client.GetFile(fileId);
        MemoryStream ms = new MemoryStream();
        if (file.FilePath != null)
            await client.DownloadFile(file.FilePath, ms);
        ms.Position = 0;
        return ms;
    }
}