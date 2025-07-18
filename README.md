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

4. Update `appsettings.json` or configure `Keys.env` with connections to APIs and database
   ```
   TELEGRAM_BOT_TOKEN
   OPENAI_API_KEY
   MINDEE_API_KEY
   DEFAULT_DB_CONNECTION
   ```

5. Run Migrator `Database.MigrationRunner` project to set up database

6. Run the Telegram bot application:
   ```
   dotnet run --project InsuranceBot.WebApi
   ```

7. (optional) Run command for Docker:
   ```
   docker compose build
   docker compose up
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

## Bot commands
- `/start` - start dialog with bot
- `/start(admin)` - start dialog with bot and set user as admin
- `/cancel` - reset process
- `/resendpolicy` - request generated file
- `/admin summary` - summary of issued policies
- `/generate_policy` - request policy generation after inserted data confirmed

## License

[MIT](LICENSE)
