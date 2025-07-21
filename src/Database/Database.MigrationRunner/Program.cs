using System.Reflection;
using dotenv.net;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
    .AddJsonFile("appsettings.json", false)
    .Build();

DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { "Keys.env" }));

ServiceCollection serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton(configuration);

serviceCollection
    .AddFluentMigratorCore()
    .ConfigureRunner(config =>
        config.AddSqlServer()
            .WithGlobalConnectionString(Environment.GetEnvironmentVariable("DEFAULT_DB_CONNECTION")!) // for local launch set value manually
            .ScanIn(Assembly.Load(configuration["Assembly:InsuranceBot"]!)).For.All())
    .AddLogging(config => config.AddFluentMigratorConsole());

ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

using (serviceProvider as IDisposable)
{
    IMigrationRunner runner = serviceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}