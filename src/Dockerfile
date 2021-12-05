#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Bootstrapper/Bootstrapper.csproj", "src/Bootstrapper/"]
COPY ["Core/Application/Application.csproj", "src/Core/Application/"]
COPY ["Core/Domain/Domain.csproj", "src/Core/Domain/"]
COPY ["Shared/Shared.DTOs/Shared.DTOs.csproj", "src/Shared/Shared.DTOs/"]
COPY ["Migrators/Migrators.MSSQL/Migrators.MSSQL.csproj", "src/Migrators/Migrators.MSSQL/"]
COPY ["Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["Migrators/Migrators.PostgreSQL/Migrators.PostgreSQL.csproj", "src/Migrators/Migrators.PostgreSQL/"]
COPY ["Migrators/Migrators.MySQL/Migrators.MySQL.csproj", "src/Migrators/Migrators.MySQL/"]
RUN dotnet restore "src/Bootstrapper/Bootstrapper.csproj"
COPY . .
WORKDIR "/src/Bootstrapper"
RUN dotnet build "Bootstrapper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bootstrapper.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StoreKit.Bootstrapper.dll"]