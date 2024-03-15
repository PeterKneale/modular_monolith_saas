FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /workspace
COPY ["MicroSaas.sln", ""]
COPY ["src/Micro.Web/Micro.Web.csproj", "src/Micro.Web/"]
COPY ["src/Micro.Cli/Micro.Cli.csproj", "src/Micro.Cli/"]
COPY ["src/Micro.Common/Micro.Common.csproj", "src/Micro.Common/"]
COPY ["src/Micro.Tenants/Micro.Tenants.csproj", "src/Micro.Tenants/"]
COPY ["src/Micro.Tenants.IntegrationEvents/Micro.Tenants.IntegrationEvents.csproj", "src/Micro.Tenants.IntegrationEvents/"]
COPY ["src/Micro.Translations/Micro.Translations.csproj", "src/Micro.Translations/"]
COPY ["src/Micro.Translations.IntegrationEvents/Micro.Translations.IntegrationEvents.csproj", "src/Micro.Translations.IntegrationEvents/"]
COPY ["tests/Micro.Common.UnitTests/Micro.Common.UnitTests.csproj", "tests/Micro.Common.UnitTests/"]
COPY ["tests/Micro.AcceptanceTests/Micro.AcceptanceTests.csproj", "tests/Micro.AcceptanceTests/"]
COPY ["tests/Micro.IntegrationTests/Micro.IntegrationTests.csproj", "tests/Micro.IntegrationTests/"]
COPY ["tests/Micro.Tenants.UnitTests/Micro.Tenants.UnitTests.csproj", "tests/Micro.Tenants.UnitTests/"]
COPY ["tests/Micro.Tenants.IntegrationTests/Micro.Tenants.IntegrationTests.csproj", "tests/Micro.Tenants.IntegrationTests/"]
COPY ["tests/Micro.Translations.UnitTests/Micro.Translations.UnitTests.csproj", "tests/Micro.Translations.UnitTests/"]
COPY ["tests/Micro.Translations.IntegrationTests/Micro.Translations.IntegrationTests.csproj", "tests/Micro.Translations.IntegrationTests/"]
RUN dotnet restore
COPY . .
WORKDIR "/workspace/"
RUN dotnet build "src/Micro.Web/Micro.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS tests

FROM build AS publish
ARG BUILD_VERSION=1.0.0
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/Micro.Web/Micro.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:Version=$BUILD_VERSION /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Micro.Web.dll"]
