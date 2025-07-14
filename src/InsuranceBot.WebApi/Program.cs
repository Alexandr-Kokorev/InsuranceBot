using InsuranceBot.Application.Commands;
using InsuranceBot.WebApi.Extensions;
using InsuranceBot.WebApi.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//builder.Services.AddHttpClient();

builder.Services
    .AddApplicationDbContext(builder.Configuration)
    .AddExternalServices(builder.Configuration)
    .AddRepositories()
    .AddServices()
    .AddHandlers();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(StartCommand).Assembly);
});

builder.Services.AddHostedService<TelegramHostedService>();

WebApplication app = builder.Build();
app.Run();