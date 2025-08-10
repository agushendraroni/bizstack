FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
WORKDIR /app
EXPOSE 5283
ENV ASPNETCORE_URLS=http://+:5283

FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src

# Copy seluruh folder services dan shared-library agar struktur path tetap
COPY services ./services
COPY shared-library ./shared-library

WORKDIR /src/services/user-service

RUN dotnet restore
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "user-service.dll"]