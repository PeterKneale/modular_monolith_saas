FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tests/Micro.Web.AcceptanceTests/Micro.Web.AcceptanceTests.csproj", "tests/Micro.Web.AcceptanceTests/"]
RUN dotnet restore "tests/Micro.Web.AcceptanceTests/Micro.Web.AcceptanceTests.csproj"
COPY . .
WORKDIR "/src/tests/Micro.Web.AcceptanceTests"
RUN dotnet build "Micro.Web.AcceptanceTests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Micro.Web.AcceptanceTests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/playwright/dotnet:v1.49.0-jammy AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Micro.Web.AcceptanceTests.dll"]
