FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Database/Database.MigrationRunner/Database.MigrationRunner.csproj", "Database/Database.MigrationRunner/"]
COPY ["Database/Database.InsuranceBot/Database.InsuranceBot.csproj", "Database/Database.InsuranceBot/"]
RUN dotnet restore "Database/Database.MigrationRunner/Database.MigrationRunner.csproj"
COPY . .
WORKDIR "/src/Database/Database.MigrationRunner"
RUN dotnet build "./Database.MigrationRunner.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Database.MigrationRunner.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Database.MigrationRunner.dll"]
