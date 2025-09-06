FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5004

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["services/product-service/product-service.csproj", "services/product-service/"]
COPY ["shared-library/shared-library.csproj", "shared-library/"]
RUN dotnet restore "services/product-service/product-service.csproj"
COPY . .
WORKDIR "/src/services/product-service"
RUN dotnet build "product-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "product-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 5004
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5004
ENTRYPOINT ["dotnet", "product-service.dll"]
