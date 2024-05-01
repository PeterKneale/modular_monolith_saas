FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Micro.Proxy/Micro.Proxy.csproj", "src/Micro.Proxy/"]
RUN dotnet restore "src/Micro.Proxy/Micro.Proxy.csproj"
COPY . .
WORKDIR "/src/src/Micro.Proxy"
RUN dotnet build "Micro.Proxy.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Micro.Proxy.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Micro.Proxy.dll"]
