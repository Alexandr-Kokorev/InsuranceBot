﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/InsuranceBot.WebApi/InsuranceBot.WebApi.csproj"
RUN dotnet build "src/InsuranceBot.WebApi/InsuranceBot.WebApi.csproj" -c Release -o /app/build
RUN dotnet publish "src/InsuranceBot.WebApi/InsuranceBot.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InsuranceBot.WebApi.dll"]