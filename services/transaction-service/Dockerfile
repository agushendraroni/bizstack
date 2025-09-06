FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["services/transaction-service/transaction-service.csproj", "services/transaction-service/"]
COPY ["shared-library/shared-library.csproj", "shared-library/"]
RUN dotnet restore "services/transaction-service/transaction-service.csproj"
COPY . .
WORKDIR "/src/services/transaction-service"
RUN dotnet build "transaction-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "transaction-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "transaction-service.dll"]
