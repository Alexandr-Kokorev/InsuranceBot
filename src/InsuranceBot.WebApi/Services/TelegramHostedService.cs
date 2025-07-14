using InsuranceBot.Telegram.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace InsuranceBot.WebApi.Services;

public class TelegramHostedService(ITelegramBotClient client, IServiceProvider scopeFactory) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        client.StartReceiving(new DefaultUpdateHandler(
                async (botClient, update, token) =>
                {
                    using IServiceScope scope = scopeFactory.CreateScope();
                    TelegramUpdateHandler handler = scope.ServiceProvider.GetRequiredService<TelegramUpdateHandler>();
                    await handler.HandleUpdateAsync(update, token);
                },
                async (botClient, exception, token) => { /* log errors */ }),
            cancellationToken: cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Bot is stopping...");
        cancellationToken.ThrowIfCancellationRequested();

        Console.WriteLine("Bot has stopped receiving updates.");

        return Task.CompletedTask;
    }
}