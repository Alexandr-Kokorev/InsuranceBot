using dotenv.net;
using InsuranceBot.Application.Commands;
using InsuranceBot.WebApi.Extensions;
using InsuranceBot.WebApi.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { "Keys.env" }));

var s = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
var sd = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
var sf = Environment.GetEnvironmentVariable("MINDEE_API_KEY");
var sv = Environment.GetEnvironmentVariable("DEFAULT_DB_CONNECTION");

builder.Services
    .AddApplicationDbContext()
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