FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
WORKDIR /app
EXPOSE 5296
ENV ASPNETCORE_URLS=http://+:5296

FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src

# Copy entire services and shared-library folders
COPY services ./services
COPY shared-library ./shared-library

WORKDIR /src/services/organization-service

RUN dotnet restore
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "organization-service.dll"]



