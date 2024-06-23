FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /workspace

# solution
COPY ["MicroSaas.sln", ""]

# cli
COPY ["src/Micro.Cli/Micro.Cli.csproj", "src/Micro.Cli/"]

# web
COPY ["src/Micro.Web/Micro.Web.csproj", "src/Micro.Web/"]
COPY ["src/Micro.Web/Files/DataProtectionKey.xml", "src/Micro.Web/Files/"]
COPY ["tests/Micro.Web.AcceptanceTests/Micro.Web.AcceptanceTests.csproj", "tests/Micro.Web.AcceptanceTests/"]

# common
COPY ["src/Micro.Common/Micro.Common.csproj", "src/Micro.Common/"]
COPY ["src/Micro.Common.Web/Micro.Common.Web.csproj", "src/Micro.Common.Web/"]
COPY ["tests/Micro.Common.UnitTests/Micro.Common.UnitTests.csproj", "tests/Micro.Common.UnitTests/"]

# system
COPY ["tests/Micro.Modules.SystemTests/Micro.Modules.SystemTests.csproj", "tests/Micro.Modules.SystemTests/"]

# module - users
COPY ["src/Micro.Users.Application/Micro.Users.Application.csproj", 							"src/Micro.Users.Application/"]
COPY ["src/Micro.Users.Domain/Micro.Users.Domain.csproj", 										"src/Micro.Users.Domain/"]
COPY ["src/Micro.Users.Infrastructure/Micro.Users.Infrastructure.csproj", 						"src/Micro.Users.Infrastructure/"]
COPY ["src/Micro.Users.Messages/Micro.Users.Messages.csproj", 									"src/Micro.Users.Messages/"]
COPY ["src/Micro.Users.Web/Micro.Users.Web.csproj", 											"src/Micro.Users.Web/"]
COPY ["tests/Micro.Users.Domain.UnitTests/Micro.Users.Domain.UnitTests.csproj", 				"tests/Micro.Users.Domain.UnitTests/"]
COPY ["tests/Micro.Users.IntegrationTests/Micro.Users.IntegrationTests.csproj", 				"tests/Micro.Users.IntegrationTests/"]

# module - tenants
COPY ["src/Micro.Tenants.Application/Micro.Tenants.Application.csproj", 						"src/Micro.Tenants.Application/"]
COPY ["src/Micro.Tenants.Domain/Micro.Tenants.Domain.csproj", 									"src/Micro.Tenants.Domain/"]
COPY ["src/Micro.Tenants.Infrastructure/Micro.Tenants.Infrastructure.csproj", 					"src/Micro.Tenants.Infrastructure/"]
COPY ["src/Micro.Tenants.Messages/Micro.Tenants.Messages.csproj",								"src/Micro.Tenants.Messages/"]
COPY ["src/Micro.Tenants.Web/Micro.Tenants.Web.csproj", 										"src/Micro.Tenants.Web/"]
COPY ["tests/Micro.Tenants.Domain.UnitTests/Micro.Tenants.Domain.UnitTests.csproj", 			"tests/Micro.Tenants.Domain.UnitTests/"]
COPY ["tests/Micro.Tenants.IntegrationTests/Micro.Tenants.IntegrationTests.csproj", 			"tests/Micro.Tenants.IntegrationTests/"]

# module - translations 
COPY ["src/Micro.Translations.Application/Micro.Translations.Application.csproj", 				"src/Micro.Translations.Application/"]
COPY ["src/Micro.Translations.Domain/Micro.Translations.Domain.csproj", 						"src/Micro.Translations.Domain/"]
COPY ["src/Micro.Translations.Infrastructure/Micro.Translations.Infrastructure.csproj", 		"src/Micro.Translations.Infrastructure/"]
COPY ["src/Micro.Translations.Messages/Micro.Translations.Messages.csproj", 					"src/Micro.Translations.Messages/"]
COPY ["src/Micro.Translations.Web/Micro.Translations.Web.csproj", 								"src/Micro.Translations.Web/"]
COPY ["tests/Micro.Translations.Domain.UnitTests/Micro.Translations.Domain.UnitTests.csproj", 	"tests/Micro.Translations.Domain.UnitTests/"]
COPY ["tests/Micro.Translations.IntegrationTests/Micro.Translations.IntegrationTests.csproj", 	"tests/Micro.Translations.IntegrationTests/"]

RUN dotnet restore
COPY . .
RUN chmod +x *.sh
WORKDIR "/workspace/"
RUN dotnet build "src/Micro.Web/Micro.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS unit-tests

FROM build AS integration-tests

FROM build AS system-tests

FROM build AS publish
ARG BUILD_VERSION=1.0.0
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/Micro.Web/Micro.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:Version=$BUILD_VERSION /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Micro.Web.dll"]
