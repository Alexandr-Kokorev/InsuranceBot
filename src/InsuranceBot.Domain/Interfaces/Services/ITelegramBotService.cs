using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace InsuranceBot.Domain.Interfaces.Services;

public interface ITelegramBotService
{
    Task SendTextAsync(long chatId, string message);
    Task SendReplyMarkupAsync(long chatId, string text, InlineKeyboardMarkup markup);
    Task SendDocumentAsync(long chatId, Stream docStream, string fileName);
    Task<Stream> GetFileStreamAsync(string fileId);
}