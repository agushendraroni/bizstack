FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5007
ENV ASPNETCORE_URLS=http://+:5007

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["services/report-service/report-service.csproj", "services/report-service/"]
COPY ["shared-library/shared-library.csproj", "shared-library/"]
RUN dotnet restore "services/report-service/report-service.csproj"
COPY . .
WORKDIR "/src/services/report-service"
RUN dotnet build "report-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "report-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "report-service.dll"]
