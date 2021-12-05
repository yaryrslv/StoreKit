#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /../src
COPY ["src/Bootstrapper/Bootstrapper.csproj", "src/Bootstrapper/"]
COPY ["src/Core/Application/Application.csproj", "src/Core/Application/"]
COPY ["src/Core/Domain/Domain.csproj", "src/Core/Domain/"]
COPY ["src/Shared/Shared.DTOs/Shared.DTOs.csproj", "src/Shared/Shared.DTOs/"]
COPY ["src/Migrators/Migrators.MSSQL/Migrators.MSSQL.csproj", "src/Migrators/Migrators.MSSQL/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Migrators/Migrators.PostgreSQL/Migrators.PostgreSQL.csproj", "src/Migrators/Migrators.PostgreSQL/"]
COPY ["src/Migrators/Migrators.MySQL/Migrators.MySQL.csproj", "src/Migrators/Migrators.MySQL/"]
RUN dotnet restore "src/Bootstrapper/Bootstrapper.csproj"
COPY . .
WORKDIR "/src/src/Bootstrapper"
RUN dotnet build "Bootstrapper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bootstrapper.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StoreKit.Bootstrapper.dll"]