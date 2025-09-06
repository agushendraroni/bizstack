FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["services/organization-service/organization-service.csproj", "services/organization-service/"]
COPY ["shared-library/shared-library.csproj", "shared-library/"]
RUN dotnet restore "services/organization-service/organization-service.csproj"
COPY . .
WORKDIR "/src/services/organization-service"
RUN dotnet build "organization-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "organization-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "organization-service.dll"]
