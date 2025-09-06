FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["services/auth-service/auth-service.csproj", "services/auth-service/"]
COPY ["shared-library/shared-library.csproj", "shared-library/"]
RUN dotnet restore "services/auth-service/auth-service.csproj"
COPY . .
WORKDIR "/src/services/auth-service"
RUN dotnet build "auth-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "auth-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5001
ENTRYPOINT ["dotnet", "auth-service.dll"]
