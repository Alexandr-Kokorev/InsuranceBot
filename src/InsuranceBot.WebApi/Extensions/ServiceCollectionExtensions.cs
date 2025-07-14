using InsuranceBot.Domain.Interfaces.Repositories;
using InsuranceBot.Domain.Interfaces.Services;
using InsuranceBot.Infrastructure.Data;
using InsuranceBot.Infrastructure.Repositories;
using InsuranceBot.Infrastructure.Services;
using InsuranceBot.Telegram.Handlers;
using Microsoft.EntityFrameworkCore;
using Mindee;
using Telegram.Bot;

namespace InsuranceBot.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration["DEFAULT_DB_CONNECTION"]));
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IPolicyRepository, PolicyRepository>()
            .AddScoped<IDocumentRepository, DocumentRepository>();
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services.AddScoped<IAuditLogService, AuditLogService>()
            .AddScoped<IErrorLogService, ErrorLogService>()
            .AddScoped<IUserStateService, UserStateService>()
            .AddScoped<IFileStorageService, FileStorageService>()
            .AddScoped<IPolicyPdfService, PolicyPdfService>()
            .AddSingleton<IApprovalQueueService, InMemoryApprovalQueueService>();
    }

    public static IServiceCollection AddExternalServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(configuration["TELEGRAM_BOT_TOKEN"]!));
        services.AddScoped<ITelegramBotService, TelegramBotService>();

        services.AddSingleton<MindeeClient>(_ => new MindeeClient(configuration["MINDEE_API_KEY"]!));
        services.AddHttpClient<IMindeeApiService, MindeeApiService>();
        services.AddScoped<IMindeeApiService, MindeeApiService>();
        
        services.AddSingleton<IOpenAiService>(provider =>
        {
            HttpClient httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
            return new OpenAiService(configuration, httpClient);
        });
        
        return services;
    }

    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        return services.AddScoped<TelegramUpdateHandler>();
    }
}