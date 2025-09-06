FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["services/customer-service/customer-service.csproj", "services/customer-service/"]
COPY ["shared-library/shared-library.csproj", "shared-library/"]
RUN dotnet restore "services/customer-service/customer-service.csproj"
COPY . .
WORKDIR "/src/services/customer-service"
RUN dotnet build "customer-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "customer-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "customer-service.dll"]
