FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tests/Micro.AcceptanceTests/Micro.AcceptanceTests.csproj", "tests/Micro.AcceptanceTests/"]
RUN dotnet restore "tests/Micro.AcceptanceTests/Micro.AcceptanceTests.csproj"
COPY . .
WORKDIR "/src/tests/Micro.AcceptanceTests"
RUN dotnet build "Micro.AcceptanceTests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Micro.AcceptanceTests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/playwright/dotnet:v1.42.0-jammy AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Micro.AcceptanceTests.dll"]
