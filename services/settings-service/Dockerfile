FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5010
ENV ASPNETCORE_URLS=http://+:5010

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["services/settings-service/settings-service.csproj", "services/settings-service/"]
COPY ["shared-library/shared-library.csproj", "shared-library/"]
RUN dotnet restore "services/settings-service/settings-service.csproj"
COPY . .
WORKDIR "/src/services/settings-service"
RUN dotnet build "settings-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "settings-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "settings-service.dll"]