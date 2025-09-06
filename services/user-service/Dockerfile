FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["services/user-service/user-service.csproj", "services/user-service/"]
COPY ["shared-library/shared-library.csproj", "shared-library/"]
RUN dotnet restore "services/user-service/user-service.csproj"
COPY . .
WORKDIR "/src/services/user-service"
RUN dotnet build "user-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "user-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 5002
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5002
ENTRYPOINT ["dotnet", "user-service.dll"]
