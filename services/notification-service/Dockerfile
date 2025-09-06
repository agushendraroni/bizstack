FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5008
ENV ASPNETCORE_URLS=http://+:5008

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["services/notification-service/notification-service.csproj", "services/notification-service/"]
COPY ["shared-library/shared-library.csproj", "shared-library/"]
RUN dotnet restore "services/notification-service/notification-service.csproj"
COPY . .
WORKDIR "/src/services/notification-service"
RUN dotnet build "notification-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "notification-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "notification-service.dll"]
