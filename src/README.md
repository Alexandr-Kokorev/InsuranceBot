# InsuranceBot

A Telegram bot application for car insurance sales built with .NET 8.0.

## Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop/) (optional, for containerized deployment)

## Setup Instructions

### Local Development

1. Clone the repository:
   ```
   git clone <repository-url>
   cd InsuranceBot
   ```

2. Restore dependencies:
   ```
   dotnet restore
   ```

3. Build the solution:
   ```
   dotnet build
   ```

4. Update `appsettings.json` or `secrets.json` with connections to APIs and database
```
TELEGRAM_BOT_TOKEN
OPENAI_API_KEY
MINDEE_API_KEY
DEFAULT_DB_CONNECTION
```

6. Run EF Core migrations on `InsuranceBot.Infrastructure` project to set up database

5. Run the Telegram bot application:
   ```
   dotnet run --project InsuranceBot.Telegram
   ```

5. Run the Web API (optional):
   ```
   dotnet run --project InsuranceBot.WebApi
   ```

## Project Structure

- `InsuranceBot.Application` - Application services and business logic
- `InsuranceBot.Domain` - Domain entities and business rules
- `InsuranceBot.Infrastructure` - External services implementation
- `InsuranceBot.Telegram` - Telegram bot interface
- `InsuranceBot.WebApi` - Web API for integration (Startup)
- `InsuranceBot.Tests` - Unit and integration tests

## Configuration

Before running the application, make sure to configure your Telegram Bot Token in the appropriate configuration file.

## License

[MIT](LICENSE)
