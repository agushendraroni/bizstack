FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["services/file-storage-service/file-storage-service.csproj", "services/file-storage-service/"]
COPY ["shared-library/shared-library.csproj", "shared-library/"]
RUN dotnet restore "services/file-storage-service/file-storage-service.csproj"
COPY . .
WORKDIR "/src/services/file-storage-service"
RUN dotnet build "file-storage-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "file-storage-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "file-storage-service.dll"]
